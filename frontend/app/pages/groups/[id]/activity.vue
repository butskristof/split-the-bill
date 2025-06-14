<template>
  <main
    v-if="group"
    class="activity"
  >
    <h1>Group activity</h1>
    <h2>Expenses</h2>
    <GroupExpense
      v-for="expense in expenses"
      :key="expense.id"
      :expense="expense"
    />
    <h2>Payments</h2>
    <GroupPayment
      v-for="payment in payments"
      :key="payment.id"
      :payment="payment"
    />
  </main>
</template>

<script setup lang="ts">
import GroupExpense from '~/components/groups/activities/expenses/GroupExpense.vue';
import GroupPayment from '~/components/groups/activities/payments/GroupPayment.vue';

const { group } = useDetailPageGroup();
const expenses = computed(() => group.value?.expenses || []);
const payments = computed(() => group.value?.payments || []);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.activity {
  @include utilities.flex-column;
}
</style>
