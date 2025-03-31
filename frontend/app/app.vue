<template>
  <UApp>
    <NuxtRouteAnnouncer />
    <NuxtLayout>
      <NuxtPage />
    </NuxtLayout>
  </UApp>
</template>

<script setup lang="ts">
useHead({
  // title: 'Dashboard',
  titleTemplate: (title) => (title ? `${title} - ` : '') + 'Split the bill',
});
</script>
