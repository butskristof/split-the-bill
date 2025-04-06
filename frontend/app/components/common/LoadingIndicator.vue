<template>
  <div class="loading-indicator">
    <UIcon
      name="i-mynaui-circle-notch"
      class="size-10 animate-spin"
    />
    <div v-if="message">{{ message }}</div>
    <div v-if="entity">Loading {{ entity }}</div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  entity?: string;
  message?: string;
}>();
</script>

<style scoped>
.loading-indicator {
  padding-block: 4rem;
  text-align: center;

  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}
</style>
