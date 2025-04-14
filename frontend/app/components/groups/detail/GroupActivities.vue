<template>
  <div class="activities">
    <div class="header">
      <h2>Activities</h2>
      <div class="actions">
        <UDropdownMenu
          :items="[
            {
              label: 'Expense',
              icon: 'i-material-symbols-payments-outline',
              onSelect: openExpense,
            },
            {
              label: 'Payment',
              icon: 'i-hugeicons-save-money-euro',
            },
          ]"
          :content="{ align: 'end', side: 'bottom' }"
        >
          <UButton
            icon="i-mynaui-plus"
            label="Add"
          />
        </UDropdownMenu>
        <UModal v-model:open="isOpenExpense">
          <template #content>
            <h2>Add new expense</h2>
          </template>
        </UModal>
      </div>
    </div>
    <GroupActivity />
    <GroupActivity />
    <GroupActivity />
  </div>
</template>

<script setup lang="ts">
import GroupActivity from '~/components/groups/detail/GroupActivity.vue';

const isOpenExpense = ref(false);
const openExpense = () => {
  isOpenExpense.value = true;
};
</script>

<style scoped>
@reference '~/assets/styles/main.css';

.header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;

  h2 {
    @apply text-xl font-bold;
  }
}
</style>
