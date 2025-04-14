<template>
  <div class="back-button">
    <UButton
      :to="to"
      icon="i-mynaui-arrow-left"
      variant="ghost"
    >
      <slot />
    </UButton>
  </div>
</template>

<script setup lang="ts">
import type { RouteLocationRaw } from '#vue-router';

defineProps<{
  to: RouteLocationRaw;
}>();
</script>
