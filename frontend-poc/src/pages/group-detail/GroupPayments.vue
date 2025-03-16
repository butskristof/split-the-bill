<template>
  <h2>Payments</h2>
  <table>
    <thead>
      <tr>
        <th>Sender</th>
        <th>Receiver</th>
        <th>Amount</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="payment in payments"
        :key="payment.id"
      >
        <td>{{ memberNames[payment.sendingMemberId] }}</td>
        <td>{{ memberNames[payment.receivingMemberId] }}</td>
        <td>{{ payment.amount }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { GroupDto } from '@/types/groups';

const props = defineProps<{
  group: GroupDto;
}>();
const payments = computed(() => props.group.payments);

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
