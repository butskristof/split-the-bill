<template>
  <div class="group-name">
    <UAvatar
      size="xl"
      class="icon"
    />
    <div class="name">{{ name }}</div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  name: string;
}>();
</script>

<style scoped>
@reference '~/assets/css/main.css';

.group-name {
  display: flex;
  gap: 1rem;
  align-items: center;

  margin-bottom: 1rem;
}

.name {
  @apply text-2xl font-bold;
}
</style>
