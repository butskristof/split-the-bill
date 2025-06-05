<template>
  <div class="activity expense">
    <div>
      <strong>{{ expense.description }}: {{ formatCurrency(expense.amount) }}</strong>
    </div>

    paid by <strong>{{ paidByMember?.name }}</strong>
    <div class="timestamp">
      {{ formatTimestamp(expense.timestamp) }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Expense, GroupMember } from '~/types';
import { formatTimestamp, formatCurrency } from '#shared/utils';

const props = defineProps<{
  expense: Expense;
}>();

const { getMember } = useDetailPageGroup();
const paidByMember = computed<GroupMember | undefined>(() =>
  getMember(props.expense.paidByMemberId),
);
</script>
