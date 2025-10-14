<template>
  <Button
    v-tippy="label"
    :icon="icon"
    :aria-label="label"
    severity="contrast"
    class="color-mode-toggle p-button"
    variant="text"
    @click="cycleColorMode"
  />
</template>

<script setup lang="ts">
import Button from 'primevue/button';

const colorMode = useColorMode();

const nextMode = computed(() => {
  // if unknown (SSR), assume we're in light mode and dark mode is next up
  if (colorMode.unknown) return 'dark';
  return colorMode.value === 'light' ? 'dark' : 'light';
});
const cycleColorMode = () => {
  // set preference, not value, so it's persisted in local storage
  colorMode.preference = nextMode.value;
};

const icon = computed(() => (nextMode.value === 'light' ? 'pi pi-sun' : 'pi pi-moon'));
const label = computed(() => (nextMode.value === 'light' ? 'Light mode' : 'Dark mode'));
</script>

<style scoped lang="scss">
// override Primevue class, used in template as well to avoid unused warning
.p-button {
  --p-button-padding-x: 0;
  --p-button-padding-y: 0;
  --p-button-icon-only-width: 1.25rem;
  height: 1.25rem;
}
</style>
