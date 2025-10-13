<template>
  <AppDialog
    header="Delete expense"
    @close="tryClose"
  >
    <div class="content">
      <p>
        Are you sure you want to delete the expense <strong>{{ props.expense.description }}</strong
        >? This action cannot be undone.
      </p>

      <div class="footer">
        <div class="messages">
          <Message
            v-if="deleteExpenseMutation.isSuccess.value"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            Expense deleted successfully!
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
            :disabled="
              deleteExpenseMutation.isPending.value || deleteExpenseMutation.isSuccess.value
            "
            @click="tryClose"
          />
          <Button
            icon="pi pi-trash"
            severity="danger"
            :loading="deleteExpenseMutation.isPending.value"
            :disabled="
              deleteExpenseMutation.isPending.value || deleteExpenseMutation.isSuccess.value
            "
            :label="deleteExpenseMutation.isPending.value ? 'Deleting...' : 'Delete'"
            @click="onDelete"
          />
        </div>
      </div>
    </div>
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import { useDeleteExpenseMutation } from '~/composables/backend-api/useDeleteExpenseMutation';
import type { FetchError } from 'ofetch';
import type { Expense, ProblemDetails } from '#shared/types/api';
import { DIALOG_SUCCESS_CLOSE_DELAY } from '#shared/constants';

const props = defineProps<{
  groupId: string;
  expense: Expense;
}>();
const emit = defineEmits<{
  close: [];
}>();
const tryClose = () => {
  if (deleteExpenseMutation.isPending.value) return;
  emit('close');
};

const deleteExpenseMutation = useDeleteExpenseMutation();
const apiErrorTitle = computed<string | null>(() => {
  if (!deleteExpenseMutation.error.value) return null;
  const problemDetails = (deleteExpenseMutation.error.value as FetchError)?.data as ProblemDetails;
  return problemDetails?.title ?? 'Something went wrong, please try again later.';
});

const onDelete = async () => {
  deleteExpenseMutation.mutate(
    { groupId: props.groupId, expenseId: props.expense.id },
    {
      onSuccess: () => {
        // Show success message briefly before navigating
        setTimeout(() => {
          navigateTo({ name: 'groups-id', params: { id: props.groupId } });
        }, DIALOG_SUCCESS_CLOSE_DELAY);
      },
    },
  );
};
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.content {
  @include utilities.flex-column;

  .footer {
    @include utilities.flex-row-justify-between;
    flex-wrap: wrap;

    .actions {
      @include utilities.flex-row-justify-end;
      margin-left: auto;
    }
  }
}
</style>
