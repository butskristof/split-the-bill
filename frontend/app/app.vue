<template>
  <UApp>
    <NuxtRouteAnnouncer />

    <div class="app">
      <AppHeader />

      <UContainer class="content-container">
        <main>
          <NuxtPage />
        </main>

        <USeparator
          icon="i-mynaui-envelope"
          color="primary"
        />

        <AppFooter class="footer" />
      </UContainer>
    </div>
  </UApp>
</template>

<script setup lang="ts">
useHead({
  // title: 'Dashboard',
  titleTemplate: (title) => (title ? `${title} - ` : '') + 'Split the bill',
});
</script>

<style scoped>
.app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.content-container {
  min-height: calc(100vh - var(--ui-header-height));
  display: flex;
  flex-direction: column;
}

main {
  margin-top: 1rem;
  margin-bottom: 1rem;
  flex-grow: 1;
}
</style>
