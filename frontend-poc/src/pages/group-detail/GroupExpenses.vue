<template>
  <h2>Expenses</h2>
  <table>
    <thead>
      <tr>
        <th>ID</th>
        <th>Description</th>
        <th>Amount</th>
        <th>Split type</th>
        <th>Paid by</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="expense in expenses"
        :key="expense.id"
      >
        <td>{{ expense.id }}</td>
        <td>{{ expense.description }}</td>
        <td>{{ expense.amount }}</td>
        <td>{{ expense.splitType }}</td>
        <td>{{ memberNames[expense.paidByMemberId] }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import { type GroupDto } from '@/types/groups';
import { computed } from 'vue';

const props = defineProps<{
  group: GroupDto;
}>();
const expenses = computed(() => props.group.expenses);
const memberNames = computed(() =>
  props.group.members.reduce(
    (result, member) => {
      result[member.id] = member.name;
      return result;
    },
    {} as Record<string, string>,
  ),
);
</script>

<style scoped>
table {
  border-collapse: collapse;
}

th,
td {
  border: 1px solid black;
  padding-left: 0.5rem;
  padding-right: 0.5rem;
}
</style>
