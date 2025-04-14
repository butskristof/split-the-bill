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
@use '~/assets/styles/utilities.scss';

pre {
  color: oklch(0.928 0.006 264.531);
  background-color: oklch(0.278 0.033 256.848);
  overflow-x: auto;
  font-weight: 400;
  font-size: 0.875rem;
  line-height: 1.5;
  border-radius: var(--ui-radius);
  padding: var(--default-spacing);
}
</style>
