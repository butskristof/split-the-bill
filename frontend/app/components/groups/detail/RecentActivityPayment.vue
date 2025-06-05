<template>
  <div class="activity payment">
    <div>
      <strong>{{ sendingMember?.name }}</strong>
      paid
      <strong>{{ formatCurrency(payment.amount) }}</strong>
      to
      <strong>{{ receivingMember?.name }}</strong>
    </div>
    <div class="timestamp">
      {{ formatTimestamp(payment.timestamp) }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { GroupMember, Payment } from '~/types';
import { formatTimestamp, formatCurrency } from '#shared/utils';

const props = defineProps<{
  payment: Payment;
}>();

const { getMember } = useDetailPageGroup();
const sendingMember = computed<GroupMember | undefined>(() =>
  getMember(props.payment.sendingMemberId),
);
const receivingMember = computed<GroupMember | undefined>(() =>
  getMember(props.payment.receivingMemberId),
);
</script>
