<template>
  <pre v-if="value">{{ JSON.stringify(value, null, 2) }}</pre>
  <pre v-if="$slots.default"><slot /></pre>
</template>

<script setup lang="ts">
defineProps<{
  value?: unknown;
}>();
</script>

<style scoped lang="scss">
@use '~/styles/utilities.scss';

pre {
  overflow-x: auto;
  font-weight: var(--font-weight-default);
  font-size: var(--text-sm);
  line-height: 1.5;
  border-radius: var(--default-radius);
  padding: var(--default-spacing);

  background-color: var(--p-surface-800);
  color: var(--p-surface-0);
}
</style>
