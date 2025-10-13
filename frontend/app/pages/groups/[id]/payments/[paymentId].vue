<template>
  <div class="payment-detail">
    <div
      v-if="payment"
      class="payment"
    >
      <div class="members-avatars">
        <MemberAvatar :member="sender" />
        <div class="pi pi-arrow-right" />
        <MemberAvatar :member="receiver" />
      </div>
      <p class="members-text">
        <strong>
          <InlineGroupMember :member="sender" />
        </strong>
        paid back
        <strong>
          <InlineGroupMember :member="receiver" />
        </strong>
      </p>

      <div class="details">
        <div class="detail-row">
          <span class="label">Amount</span>
          <span class="value">{{ formatCurrency(payment.amount) }}</span>
        </div>

        <div class="detail-row">
          <span class="label">Date & time</span>
          <span class="value">{{ formatDateTime(payment.timestamp) }}</span>
        </div>
      </div>
    </div>

    <Message
      v-else
      severity="warn"
      >Payment not found</Message
    >
  </div>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import { formatCurrency, formatDateTime } from '#shared/utils';
import type { Payment } from '#shared/types/api';
import type { Member } from '#shared/types/member';

const { payments, members } = useDetailPageGroup();
const route = useRoute();
const paymentId = computed(() => route.params.paymentId as string);

const payment = computed<Payment | null>(
  () => (payments.value?.find((p) => p.id === paymentId.value) as Payment) ?? null,
);
const sender = computed<Member | null>(() =>
  payment.value?.sendingMemberId ? getMember(payment.value.sendingMemberId) : null,
);
const receiver = computed<Member | null>(() =>
  payment.value?.receivingMemberId ? getMember(payment.value.receivingMemberId) : null,
);

const getMember = (memberId: string): Member | null =>
  (members.value?.find((m) => m.id === memberId) as Member) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.payment {
  @include utilities.flex-column;

  .members-avatars {
    @include utilities.flex-row-align-center;
    justify-content: center;
    margin-bottom: calc(var(--default-spacing) / 2);
  }

  .members-text {
    text-align: center;
  }
}

.details {
  @include utilities.flex-column;

  .detail-row {
    @include utilities.flex-row-justify-between;

    .label {
      @include utilities.muted;
    }
  }
}
</style>
