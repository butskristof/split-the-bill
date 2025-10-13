<template>
  <Card class="payment">
    <template #content>
      <div class="members-avatars">
        <MemberAvatar :member="sender" />
        <div class="pi pi-arrow-right" />
        <MemberAvatar :member="receiver" />
      </div>
      <div class="members-text">
        <strong>
          <InlineGroupMember :member="sender" />
        </strong>
        paid back
        <strong>
          <InlineGroupMember :member="receiver" />
        </strong>
      </div>
      <div class="amount-timestamp">
        <div class="amount">
          <strong>{{ formatCurrency(payment.amount) }}</strong>
        </div>
        <div class="timestamp">
          {{ formatDateTime(payment.timestamp) }}
        </div>
      </div>
    </template>
  </Card>
</template>

<script setup lang="ts">
import type { Payment } from '#shared/types/api';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import { formatCurrency, formatDateTime } from '#shared/utils';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';

type Member = {
  id: string;
  name: string;
};

const props = defineProps<{
  payment: Payment;
  members: Member[];
}>();

const sender = computed<Member | null>(() => getMember(props.payment.sendingMemberId));
const receiver = computed<Member | null>(() => getMember(props.payment.receivingMemberId));

const getMember = (memberId: string): Member | null =>
  props.members?.find((m) => m.id === memberId) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

:deep(.p-card-content) {
  @include utilities.flex-column(false);

  .members-avatars {
    @include utilities.flex-row-align-center;
    justify-content: center;
    margin-bottom: calc(var(--default-spacing) / 2);
  }

  .amount-timestamp {
    @include utilities.flex-row-justify-between(false);
    align-items: flex-end; // put timestamp on same baseline as amount

    .amount {
      font-size: 110%;
    }

    .timestamp {
      font-size: 0.75rem;
      @include utilities.muted;
    }
  }
}
</style>
