<template>
  <ClientOnly>
    <UButton
      v-if="!isUnknown"
      :icon="icon"
      aria-label="Toggle color mode"
      color="neutral"
      variant="ghost"
      @click="toggleColorMode"
    />
  </ClientOnly>
</template>

<script setup lang="ts">
const colorMode = useColorMode();
const isUnknown = computed(() => colorMode.value === 'unknown');
const isDark = computed(() => colorMode.value === 'dark');
const icon = computed(() => (isDark.value ? 'i-mynaui-sun' : 'i-mynaui-moon'));

const toggleColorMode = () => {
  colorMode.value = isDark.value ? 'light' : 'dark';
};
</script>
