<template>
  <Dialog
    visible
    modal
    header="Create new group"
    :style="{ width: '95%', maxWidth: '750px' }"
    :draggable="false"
    @update:visible="onUpdateVisible"
  >
    <form @submit.prevent="onFormSubmit">
      <div class="form-field">
        <label for="name">Group name</label>
        <InputText
          id="name"
          v-model.trim="r$.$value.name"
          type="text"
          :invalid="r$.name.$error"
          :disabled="formDisabled"
          autofocus
        />
        <div
          v-if="r$.name.$error"
          class="errors"
        >
          <Message
            v-for="error in r$.name.$errors"
            :key="error"
            severity="error"
            size="small"
            variant="simple"
            >{{ error }}</Message
          >
        </div>
      </div>
      <div class="form-footer">
        <div class="message">
          <Message
            v-if="isCreated"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            New group created successfully!
          </Message>
          <Message
            v-if="generalError"
            severity="error"
            variant="simple"
          >
            {{ generalError }}
          </Message>
        </div>
        <div class="actions">
          <Button
            label="Cancel"
            severity="secondary"
            :disabled="formDisabled"
            @click="tryClose"
          />
          <Button
            type="submit"
            icon="pi pi-save"
            :loading="isSubmitting"
            :disabled="formDisabled"
            :label="isSubmitting ? 'Saving...' : 'Save'"
          />
        </div>
      </div>
    </form>
  </Dialog>
</template>

<script setup lang="ts">
import { maxLength, required } from '@regle/rules';
import type { components } from '#open-fetch-schemas/backend-api';

const emit = defineEmits<{
  close: [created: boolean];
}>();
const onUpdateVisible = (newValue: boolean) => {
  // visible false -> close dialog
  if (!newValue) tryClose();
};
const tryClose = () => {
  // should either not be touched by user or confirmed they want to close and lose changes
  if (
    !r$.$anyDirty ||
    confirm('There are unsaved changes. Are you sure you want to close this dialog?')
  ) {
    emit('close', false);
  }
};

//#region form

const { $backendApi } = useNuxtApp();
type CreateGroupRequest = components['schemas']['CreateGroup.Request'];
type ValidationProblemDetails = components['schemas']['HttpValidationProblemDetails'];

const externalErrors = ref<Record<string, string[]>>({});
const generalError = ref<string>();
const isCreated = ref(false);
const isSubmitting = ref(false);
const formDisabled = computed(() => isSubmitting.value || isCreated.value);

const { r$ } = useRegle(
  { name: '' },
  {
    name: { required, maxLength: maxLength(512) },
  },
  { externalErrors },
);

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  // Clear previous errors
  generalError.value = undefined;
  isSubmitting.value = true;

  try {
    const requestBody: CreateGroupRequest = {
      name: data.name,
    };
    // submit to api & map errors
    const response = await $backendApi('/Groups', {
      method: 'POST',
      body: requestBody,
    });

    isCreated.value = true;
    await navigateTo({ name: 'groups-id', params: { id: response.id } });
  } catch (error) {
    console.error(error);

    const errorData = (error as { data?: ValidationProblemDetails })?.data;

    if (errorData?.status === 400 && errorData.errors) {
      // Handle validation errors - map to form fields
      const mappedErrors: Record<string, string[]> = {};
      for (const [key, messages] of Object.entries(errorData.errors)) {
        // Convert PascalCase (Name) to camelCase (name)
        const fieldName = key.charAt(0).toLowerCase() + key.slice(1);
        mappedErrors[fieldName] = messages;
      }
      externalErrors.value = mappedErrors;
    }

    if (errorData?.title) {
      // Handle other API errors (500, 404, etc.) with title
      generalError.value = errorData.title;
    } else {
      // Fallback for unknown errors
      generalError.value = 'Something went wrong. Please try again.';
    }
  } finally {
    isSubmitting.value = false;
  }
};

//#endregion

// TODO in dialogs later
// - accessibility (aria-busy, aria-describedby, ...)
// - keep modal open to show succes (and maybe toast?), then navigate away after some time
// - extract common form logic
// - extract common UI and setup to app dialog
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

form {
  @include utilities.flex-column;

  .form-field {
    @include utilities.flex-column(false);
    gap: calc(var(--default-spacing) / 2);
  }

  .form-footer {
    @include utilities.flex-row-justify-between;
    flex-wrap: wrap;

    .actions {
      @include utilities.flex-row-justify-end;
      margin-left: auto;
    }
  }
}
</style>
