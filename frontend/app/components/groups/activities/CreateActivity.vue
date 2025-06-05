<template>
  <Button
    icon="pi pi-plus"
    label="Add"
    @click="openCreateExpense"
  />

  <CreateExpenseModal
    v-if="showCreateExpense"
    @close="closeCreateExpense"
  />
</template>

<script setup lang="ts">
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

<style scoped lang="scss"></style>
