<template>
  <AppModal
    header="Create new group"
    @close="$emit('close', false)"
  >
    <Form
      v-slot="$form"
      :initial-values="initialValues"
      :resolver
      class="form"
      @submit="onFormSubmit"
    >
      <div class="form-field">
        <FormFieldLabel
          required
          html-for="name"
        >
          Name
        </FormFieldLabel>
        <InputText
          id="name"
          name="name"
          type="text"
          :disabled="isSubmitting"
          fluid
        />
        <Message
          v-if="$form.name?.invalid"
          severity="error"
          size="small"
          variant="simple"
        >
          {{ $form.name.error?.message }}
        </Message>
      </div>

      <Message
        v-if="isError"
        severity="error"
        icon="pi pi-exclamation-circle"
        >Failed to create a new group, please review any errors and try again later.</Message
      >

      <div class="actions">
        <Button
          icon="pi pi-times"
          label="Cancel"
          severity="secondary"
          :disabled="isSubmitting"
          @click="$emit('close', false)"
        />
        <Button
          type="submit"
          icon="pi pi-save"
          label="Save"
          :loading="isSubmitting"
          :disabled="isSubmitting"
        />
      </div>
    </Form>
  </AppModal>
</template>

<script setup lang="ts">
import * as v from 'valibot';
import AppModal from '~/components/common/AppModal.vue';
import { valibotResolver } from '@primevue/forms/resolvers/valibot';
import FormFieldLabel from '~/components/form/FormFieldLabel.vue';
import type { FormSubmitEvent } from '@primevue/forms';

const { $backendApi } = useNuxtApp();
const toast = useToast();

const emit = defineEmits<{
  close: [created: boolean];
}>();

const createGroupSchema = v.object({
  name: v.pipe(
    v.string('Group name must be a string'),
    v.trim(),
    v.nonEmpty('Group name is required'),
  ),
});
type CreateGroupForm = v.InferInput<typeof createGroupSchema>;
const resolver = valibotResolver(createGroupSchema);

const initialValues: CreateGroupForm = {
  name: '',
};

const isSubmitting = ref(false);
const isError = ref(false);

const onFormSubmit = async ({ valid, values }: FormSubmitEvent<CreateGroupForm>): Promise<void> => {
  if (!valid) return;

  // Clear previous errors and set submitting state
  isError.value = false;
  isSubmitting.value = true;

  try {
    const response = await $backendApi('/Groups', {
      method: 'POST',
      body: values,
    });

    toast.add({
      severity: 'success',
      summary: 'Group created successfully',
    });
    emit('close', true);
    await navigateTo({ name: 'groups-id', params: { id: response.id } });
  } catch (error) {
    isError.value = true;
    console.error(error);
  } finally {
    isSubmitting.value = false;
  }
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.form {
  @include utilities.flex-column;
}

.form-field {
  @include utilities.flex-column(false);
  gap: calc(var(--default-spacing) / 2);
}

.actions {
  @include utilities.flex-row-justify-between-align-center;
  margin-top: var(--default-spacing);
}
</style>
