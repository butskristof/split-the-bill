<template>
  <div class="loading-indicator">
    <i class="pi pi-spin pi-spinner" />
    <div v-if="message">{{ message }}</div>
    <div v-if="entity">Loading {{ entity }}</div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  message?: string;
  entity?: string;
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.loading-indicator {
  padding-block: calc(var(--default-spacing) * 4);
  text-align: center;
  @include utilities.flex-column(false);
  align-items: center;
  justify-content: center;
  gap: 1rem;

  .pi {
    font-size: 2rem;
  }
}
</style>
