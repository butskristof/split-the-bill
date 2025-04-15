<template>
  <UDropdownMenu
    :items="menuItems"
    :content="{
      align: 'end',
      side: 'bottom',
    }"
  >
    <UButton
      label="Add"
      icon="i-mynaui-plus"
    />
  </UDropdownMenu>
  <EditExpense v-model="isOpenAddExpense" />
</template>

<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui';
import EditExpense from '~/components/groups/activities/expenses/EditExpense.vue';

const menuItems: DropdownMenuItem[] = [
  {
    label: 'Add expense',
    icon: 'i-mynaui-credit-card-plus',
    onSelect: () => (isOpenAddExpense.value = true),
  },
  {
    label: 'Add payment',
    icon: 'i-hugeicons-wallet-done-02',
  },
];

const isOpenAddExpense = ref<boolean>(false);
</script>
