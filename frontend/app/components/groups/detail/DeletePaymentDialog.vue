<template>
  <AppDialog
    header="Delete payment"
    @close="tryClose"
  >
    <div class="content">
      <p>
        Are you sure you want to delete this payment from
        <strong><InlineGroupMember :member-id="payment.sendingMemberId" /></strong> to
        <strong><InlineGroupMember :member-id="payment.receivingMemberId" /></strong>? This action
        cannot be undone.
      </p>

      <div class="footer">
        <div class="messages">
          <Message
            v-if="deletePaymentMutation.isSuccess.value"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            Payment deleted successfully!
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
              deletePaymentMutation.isPending.value || deletePaymentMutation.isSuccess.value
            "
            @click="tryClose"
          />
          <Button
            icon="pi pi-trash"
            severity="danger"
            :loading="deletePaymentMutation.isPending.value"
            :disabled="
              deletePaymentMutation.isPending.value || deletePaymentMutation.isSuccess.value
            "
            :label="deletePaymentMutation.isPending.value ? 'Deleting...' : 'Delete'"
            @click="onDelete"
          />
        </div>
      </div>
    </div>
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import { useDeletePaymentMutation } from '~/composables/backend-api/useDeletePaymentMutation';
import type { FetchError } from 'ofetch';
import type { Payment, ProblemDetails } from '#shared/types/api';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import { DIALOG_SUCCESS_CLOSE_DELAY } from '#shared/constants';

const props = defineProps<{
  groupId: string;
  payment: Payment;
}>();
const emit = defineEmits<{
  close: [];
}>();
const tryClose = () => {
  if (deletePaymentMutation.isPending.value) return;
  emit('close');
};

const deletePaymentMutation = useDeletePaymentMutation();
const apiErrorTitle = computed<string | null>(() => {
  if (!deletePaymentMutation.error.value) return null;
  const problemDetails = (deletePaymentMutation.error.value as FetchError)?.data as ProblemDetails;
  return problemDetails?.title ?? 'Something went wrong, please try again later.';
});

const onDelete = async () => {
  deletePaymentMutation.mutate(
    { groupId: props.groupId, paymentId: props.payment.id },
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
