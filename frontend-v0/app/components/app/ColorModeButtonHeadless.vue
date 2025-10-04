<template>
  <ClientOnly>
    <slot
      v-if="!isUnknown"
      :color-mode="colorMode"
      :is-unknown="isUnknown"
      :is-dark="isDark"
      :is-light="isLight"
      :icon="icon"
      :toggle-color-mode="toggleColorMode"
      :set-color-mode="setColorMode"
      :next-mode="nextMode"
    />
  </ClientOnly>
</template>

<script setup lang="ts">
const colorMode = useColorMode();

const isUnknown = computed(() => colorMode.value === 'unknown');
const isDark = computed(() => colorMode.value === 'dark');
const isLight = computed(() => colorMode.value === 'light');
const icon = computed(() => `pi pi-${isDark.value ? 'sun' : 'moon'}`);

const nextMode = computed(() => (isDark.value ? 'light' : 'dark'));

const toggleColorMode = () => setColorMode(nextMode.value);

const setColorMode = (mode: 'light' | 'dark') => {
  colorMode.preference = mode;
};
</script>
