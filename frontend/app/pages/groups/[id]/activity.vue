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
    <PreformattedText :value="payments" />
    <AppCard />
  </main>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import GroupExpense from '~/components/groups/activities/expenses/GroupExpense.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';

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
