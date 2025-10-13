<template>
  <Card class="payment">
    <template #content>
      <div class="card-content">
        <div class="members">
          <div class="sender">
            <MemberAvatar :member="sender" />
          </div>
          <div class="pi pi-arrow-right" />
          <div class="receiver">
            <MemberAvatar :member="receiver" />
          </div>
        </div>
      </div>
      <div class="amount-timestamp">
        <div class="amount">
          <strong> {{ payment.amount.toFixed(2) }}</strong> euro
        </div>
        <div class="timestamp">
          {{ new Date(payment.timestamp).toLocaleString() }}
        </div>
      </div>
    </template>
  </Card>
</template>

<script setup lang="ts">
import type { Payment } from '#shared/types/api';
import MemberAvatar from '~/components/common/MemberAvatar.vue';

type Member = {
  id: string;
  name: string;
};

const props = defineProps<{
  payment: Payment;
  members: Member[];
}>();

const sender = computed(() => getMember(props.payment.sendingMemberId));
const receiver = computed(() => getMember(props.payment.receivingMemberId));

const getMember = (memberId: string): Member | null =>
  props.members?.find((m) => m.id === memberId) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.card-content {
  @include utilities.flex-column(false);
}

.members {
  @include utilities.flex-row-align-center;
  justify-content: center;
  margin-bottom: calc(var(--default-spacing) / 2);
}

.amount-timestamp {
  @include utilities.flex-row-justify-between(false);
  align-items: flex-end;
  flex-wrap: wrap;

  .amount {
    font-size: 110%;
  }

  .timestamp {
    font-size: 0.75rem;
    color: var(--p-surface-500);
  }
}
</style>
