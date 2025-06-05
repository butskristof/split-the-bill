<template>
  <AppCard class="recent-activity">
    <div class="header">
      <h2>Recent activity</h2>
      <Button
        icon="pi pi-plus"
        label="Add"
        @click="openCreateExpense"
      />
      <CreateExpenseModal
        v-if="showCreateExpense"
        @close="closeCreateExpense"
      />
    </div>
  </AppCard>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import CreateExpenseModal from '~/components/groups/activities/CreateExpenseModal.vue';

const showCreateExpense = ref<boolean>(false);
const openCreateExpense = (): void => {
  showCreateExpense.value = true;
};
const closeCreateExpense = (created: boolean): void => {
  showCreateExpense.value = false;
  if (created) {
    // Handle the created expense logic here, e.g., refresh the activity list
  }
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.header {
  @include utilities.flex-row-justify-between-align-center;
}
</style>
