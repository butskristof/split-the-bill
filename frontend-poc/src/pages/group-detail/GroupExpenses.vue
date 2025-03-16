<template>
  <h2>Expenses</h2>
  <div
    v-for="expense in expenses"
    :key="expense.id"
    class="expense"
  >
    <div>{{ JSON.stringify(expense, null, 2) }}</div>
  </div>
</template>

<script setup lang="ts">
import type { GroupDto } from '@/types/groups';
import { computed } from 'vue';

const props = defineProps<{
  group: GroupDto;
}>();
const expenses = computed(() => props.group.expenses);
</script>

<style scoped></style>
