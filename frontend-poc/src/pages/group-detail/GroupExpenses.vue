<template>
  <div class="expenses">
    <table>
      <thead>
        <tr>
          <th>Description</th>
          <th>Amount</th>
          <th>Split type</th>
          <th>Paid by</th>
          <th>Participants</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="expense in expenses"
          :key="expense.id"
        >
          <td>{{ expense.description }}</td>
          <td>{{ expense.amount }}</td>
          <td>{{ ExpenseSplitType[expense.splitType] }}</td>
          <td>{{ memberNames[expense.paidByMemberId] }}</td>
          <td>{{ expense.participants.map((p) => memberNames[p.memberId]).join(', ') }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { type GroupDto, ExpenseSplitType } from '@/types/groups.d.ts';
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
.expenses {
  margin-bottom: 1rem;
}
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
