<template>
  <div class="expense-detail-page">
    <AppPageBackButton :to="{ name: 'groups-id-activity', params: { id: group.id } }" />
    <GroupExpense :expense />
    <div class="delete-expense">
      <Button
        icon="pi pi-trash"
        label="Delete expense"
        severity="danger"
        variant="text"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import GroupExpense from '~/components/groups/activities/expenses/GroupExpense.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';

const route = useRoute();
const expenseId: string = route.params.expenseId as string;

const { group } = useDetailPageGroup();
const expense = computed(() => group.value?.expenses?.find((e) => e.id === expenseId));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.expense-detail-page {
  @include utilities.flex-column;
}

.delete-expense {
  @include utilities.flex-row-justify-end;
}
</style>
