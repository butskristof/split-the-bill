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
import type { Payment } from './types';
import { formatTimestamp, formatCurrency } from '#shared/utils';

const props = defineProps<{
  payment: Payment;
}>();

const { getMember } = useGroupMembers();
const sendingMember = computed(() => getMember(props.payment.sendingMemberId));
const receivingMember = computed(() => getMember(props.payment.receivingMemberId));
</script>
