<template>
  <NuxtLink :to>
    <Button
      class="back-button"
      label="Back to overview"
      icon="pi pi-arrow-left"
      variant="text"
      severity="secondary"
    />
  </NuxtLink>
</template>

<script setup lang="ts">
import type { RouteLocationRaw } from '#vue-router';

defineProps<{
  to: RouteLocationRaw;
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.back-button {
  margin-top: calc(var(--default-spacing) * -1);
  margin-left: calc(var(--default-spacing) * -0.5);

  &:hover,
  &:active,
  &:focus {
    background: transparent !important;
    color: var(--p-surface-600) !important;
    @include utilities.dark-mode {
      color: var(--p-surface-300) !important;
    }
  }

  &:active,
  &:focus {
    color: var(--p-surface-800) !important;
    @include utilities.dark-mode {
      color: var(--p-surface-100) !important;
    }
  }
}
</style>
