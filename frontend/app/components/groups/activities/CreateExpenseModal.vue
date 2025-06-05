<template>
  <AppModal
    header="Add new expense"
    @close="closeModal"
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
          html-for="description"
        >
          Description
        </FormFieldLabel>
        <InputText
          id="description"
          name="description"
          type="text"
          :disabled="isSubmitting"
          fluid
        />
        <Message
          v-if="$form.description?.invalid"
          severity="error"
          size="small"
          variant="simple"
        >
          {{ $form.description.error?.message }}
        </Message>
      </div>

      <Message
        v-if="isError"
        severity="error"
        icon="pi pi-exclamation-circle"
        >Failed to add a new expense, please review any errors and try again later.</Message
      >

      <div class="actions">
        <Button
          icon="pi pi-times"
          label="Cancel"
          severity="secondary"
          :disabled="isSubmitting"
          @click="() => closeModal(false)"
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
import AppModal from '~/components/common/AppModal.vue';
import * as v from 'valibot';
import { valibotResolver } from '@primevue/forms/resolvers/valibot';
import type { FormSubmitEvent } from '@primevue/forms';
import FormFieldLabel from '~/components/form/FormFieldLabel.vue';

const emit = defineEmits<{
  close: [created: boolean];
}>();
const closeModal = (created: boolean = false): void => {
  emit('close', created);
};

/*
public string? Description { get; init; }
public Guid? PaidByMemberId { get; init; }
public DateTimeOffset? Timestamp { get; init; }
public decimal? Amount { get; init; }
 */
const createExpenseSchema = v.object({
  description: v.pipe(
    v.string('Description must be a string'),
    v.trim(),
    v.nonEmpty('Description is required'),
  ),
});
type CreateExpenseForm = v.InferInput<typeof createExpenseSchema>;
const resolver = valibotResolver(createExpenseSchema);

const initialValues: CreateExpenseForm = {
  description: '',
};

const isSubmitting = ref<boolean>(false);
const isError = ref<boolean>(false);

const onFormSubmit = async ({
  valid,
  values,
}: FormSubmitEvent<CreateExpenseForm>): Promise<void> => {
  if (!valid) return;

  // Clear previous errors and set submitting state
  isError.value = false;
  isSubmitting.value = true;
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.actions {
  @include utilities.flex-row-justify-between-align-center;
  margin-top: var(--default-spacing);
}
</style>
