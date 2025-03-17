<template>
  <div class="member">
    <h3>{{ member.name }} ({{ member.id }})</h3>
    <div class="row">
      <div class="totals">
        <h4>Totals</h4>
        <p>Total Expense Amount: {{ member.totalExpenseAmount }}</p>
        <p>Total Expense Paid Amount: {{ member.totalExpensePaidAmount }}</p>
        <p>
          Total Expense Amount Paid By Other Members:
          {{ member.totalExpenseAmountPaidByOtherMembers }}
        </p>
        <p>Total Payment Received Amount: {{ member.totalPaymentReceivedAmount }}</p>
        <p>Total Payment Sent Amount: {{ member.totalPaymentSentAmount }}</p>
        <p>Total Amount Owed: {{ member.totalAmountOwed }}</p>
        <p>Total Amount Owed To Other Members: {{ member.totalAmountOwedToOtherMembers }}</p>
        <p v-if="false">Total Balance: {{ member.totalBalance }}</p>
        <p>
          <strong>
            Balance:
            <template v-if="member.totalBalance > 0">
              {{ member.name }} is owed {{ member.totalBalance }}
            </template>
            <template v-else-if="member.totalBalance == 0">
              {{ member.name }} is settled up!
            </template>
            <template v-else-if="member.totalBalance < 0">
              {{ member.name }} owes {{ member.totalBalance * -1 }}
            </template>
          </strong>
        </p>
      </div>
      <div class="relations">
        <h4>To other members</h4>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { MemberDto } from '@/types/groups';

const { member } = defineProps<{
  member: MemberDto;
}>();
</script>

<style scoped>
.member {
  margin-bottom: 1rem;
}

p {
  margin-bottom: 0;
}

.row {
  display: flex;
  flex-direction: row;
  gap: 1rem;

  > * {
    width: 50%;
  }
}
</style>
