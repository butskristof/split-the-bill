<template>
  <footer>
    <div class="left">
      <p>&copy; Split the bill 2025</p>
    </div>

    <div class="right">
      <UButton
        :icon="colorModeIcon"
        aria-label="Toggle color mode"
        color="neutral"
        variant="ghost"
        @click="toggleColorMode"
      />

      <UButton
        to="https://github.com/butskristof/split-the-bill"
        target="_blank"
        icon="i-mynaui-brand-github"
        aria-label="GitHub"
        color="neutral"
        variant="ghost"
      />
    </div>
  </footer>
</template>

<script setup lang="ts">
//#region color mode

const colorMode = useColorMode();
const colorModeIcon = computed(() =>
  colorMode.value === 'dark' ? 'i-mynaui-sun' : 'i-mynaui-moon',
);
const toggleColorMode = () => {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark';
};

//#endregion
</script>

<style scoped>
footer {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-top: 1rem;
  margin-bottom: 2rem;
}
</style>
