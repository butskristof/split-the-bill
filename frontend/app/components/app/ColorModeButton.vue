<template>
  <ClientOnly>
    <Button
      v-if="!isUnknown"
      :icon="icon"
      label="Toggle color mode"
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
