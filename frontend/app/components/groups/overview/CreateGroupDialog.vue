<template>
  <AppDialog
    header="Create new group"
    @close="tryClose"
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
            v-if="apiErrorTitle"
            severity="error"
            variant="simple"
          >
            {{ apiErrorTitle }}
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
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import { maxLength, required, string, minLength } from '@regle/rules';
import { inferRules } from '@regle/core';
import { mapProblemDetailsErrorsToExternalErrors } from '#shared/utils';
import { useCreateGroupMutation } from '~/composables/backend-api/useCreateGroupMutation';
import type { FetchError } from 'ofetch';
import type { CreateGroupRequest } from '#shared/types/api';
import { DIALOG_SUCCESS_CLOSE_DELAY } from '#shared/constants';

const emit = defineEmits<{
  close: [];
}>();
const tryClose = () => {
  // should either not be touched by user or confirmed they want to close and lose changes
  if (
    isCreated.value ||
    !r$.$anyDirty ||
    confirm('There are unsaved changes. Are you sure you want to close this dialog?')
  ) {
    emit('close');
  }
};

//#region form

const createGroupMutation = useCreateGroupMutation();
const isSubmitting = computed(() => createGroupMutation.isPending.value);
const isCreated = computed(() => createGroupMutation.isSuccess.value);

const formDisabled = computed<boolean>(() => isSubmitting.value || isCreated.value);
const apiErrorTitle = computed<string | null>(() => {
  if (!createGroupMutation.error.value) return null;
  const problemDetails = (createGroupMutation.error.value as FetchError)?.data as ProblemDetails;
  return problemDetails?.title ?? 'Something went wrong, please try again later.';
});
const externalErrors = computed<Record<string, string[]>>(() => {
  if (!createGroupMutation.error.value) return {};
  const problemDetails = (createGroupMutation.error.value as FetchError)
    ?.data as ValidationProblemDetails;
  if (!problemDetails?.errors) return {};
  return mapProblemDetailsErrorsToExternalErrors(problemDetails.errors);
});

type FormState = {
  name: string;
};
const formState = ref<FormState>({ name: '' });
const formSchema = computed(() =>
  inferRules(formState, {
    name: {
      string,
      required,
      minLength: minLength(1),
      maxLength: maxLength(512),
    },
  }),
);
const { r$ } = useRegle(formState, formSchema, { externalErrors });

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  const requestBody: CreateGroupRequest = {
    name: data.name,
  };

  createGroupMutation.mutate(requestBody, {
    onSuccess: (response) => {
      setTimeout(
        () => navigateTo({ name: 'groups-id', params: { id: response.id } }),
        DIALOG_SUCCESS_CLOSE_DELAY,
      );
    },
  });
};

//#endregion
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
