<template>
  <GroupExpense :expense />
</template>

<script setup lang="ts">
import GroupExpense from '~/components/groups/activities/expenses/GroupExpense.vue';

const route = useRoute();
const expenseId: string = route.params.expenseId as string;

const { group } = useDetailPageGroup();
const expense = computed(() => group.value?.expenses?.find((e) => e.id === expenseId));
</script>

<style scoped lang="scss"></style>
