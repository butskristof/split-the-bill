<template>
  <ClientOnly>
    <div class="color-mode">
      <p>Current color mode: {{ colorMode.value }}</p>
      <Button
        v-if="!isUnknown"
        label="toggle color mode"
        @click="toggleColorMode"
      />
    </div>
  </ClientOnly>
</template>

<script setup lang="ts">
const colorMode = useColorMode();
const isUnknown = computed(() => colorMode.value === 'unknown');
const isDark = computed(() => colorMode.value === 'dark');
// const icon = computed(() => (isDark.value ? 'i-mynaui-sun' : 'i-mynaui-moon'));

const toggleColorMode = () => {
  colorMode.value = isDark.value ? 'light' : 'dark';
};
</script>
