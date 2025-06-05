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

      <div class="form-field">
        <FormFieldLabel
          required
          html-for="paid_by_member_id"
        >
          Paid by
        </FormFieldLabel>
        <Select
          id="paid_by_member_id"
          name="paidByMemberId"
          :options="members"
          :disabled="isSubmitting"
          option-label="name"
          option-value="id"
          show-clear
          fluid
        />
        <Message
          v-if="$form.paidByMemberId?.invalid"
          severity="error"
          size="small"
          variant="simple"
        >
          {{ $form.paidByMemberId.error?.message }}
        </Message>
      </div>

      <div class="form-field">
        <FormFieldLabel
          required
          html-for="amount"
        >
          Amount
        </FormFieldLabel>
        <InputNumber
          input-id="amount"
          name="amount"
          :disabled="isSubmitting"
          mode="currency"
          currency="EUR"
          fluid
        />
        <Message
          v-if="$form.amount?.invalid"
          severity="error"
          size="small"
          variant="simple"
        >
          {{ $form.amount.error?.message }}
        </Message>
      </div>

      <div class="form-field">
        <FormFieldLabel
          required
          html-for="timestamp"
        >
          Timestamp
        </FormFieldLabel>
        <DatePicker
          input-id="timestamp"
          name="timestamp"
          :disabled="isSubmitting"
          show-time
          hour-format="24"
          fluid
        />
        <Message
          v-if="$form.timestamp?.invalid"
          severity="error"
          size="small"
          variant="simple"
        >
          {{ $form.timestamp.error?.message }}
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
import { ExpenseSplitType } from '~/enums';

const { $backendApi } = useNuxtApp();
const toast = useToast();

const emit = defineEmits<{
  close: [created: boolean];
}>();
const closeModal = (created: boolean = false): void => {
  emit('close', created);
};

const { members, group } = useDetailPageGroup();

const createExpenseSchema = v.object({
  description: v.pipe(
    v.string('Description must be a string'),
    v.trim(),
    v.nonEmpty('Description is required'),
  ),
  paidByMemberId: v.pipe(
    v.string('Paid by must be a string'),
    v.trim(),
    v.nonEmpty('Paid by is required'),
  ),
  amount: v.number('Amount must be a number'),
  timestamp: v.date('Timestamp must be a valid date'),
});
type CreateExpenseForm = v.InferInput<typeof createExpenseSchema>;
const resolver = valibotResolver(createExpenseSchema);

const initialValues: CreateExpenseForm = {
  description: '',
  paidByMemberId: '',
  amount: '',
  timestamp: new Date(),
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
  try {
    await $backendApi('/Groups/{groupId}/expenses', {
      method: 'POST',
      path: {
        groupId: group.value!.id!,
      },
      body: {
        groupId: group.value!.id!,
        ...values,
        splitType: ExpenseSplitType.Evenly,
        participants: members.value.map((m) => ({ memberId: m.id })),
      },
    });

    toast.add({
      severity: 'success',
      summary: 'Group created successfully',
    });
    closeModal(true);
  } catch (error) {
    isError.value = true;
    console.error(error);
  } finally {
    isSubmitting.value = false;
  }
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.form {
  @include utilities.flex-column;
}

.actions {
  @include utilities.flex-row-justify-between-align-center;
  margin-top: var(--default-spacing);
}
</style>
