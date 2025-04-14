<template>
  <UDropdownMenu
    :items="items"
    :content="{
      align: 'end',
      side: 'bottom',
    }"
  >
    <UButton
      :label="label"
      :icon="icon"
    />
  </UDropdownMenu>
</template>

<script setup lang="ts">
import type { DropdownMenuItem } from '#ui/components/DropdownMenu.vue';

defineProps<{
  label?: string;
  icon?: string;
  items: DropdownMenuItem[];
}>();
</script>

<style scoped lang="scss"></style>
