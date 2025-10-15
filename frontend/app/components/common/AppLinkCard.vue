<template>
  <NuxtLink
    :to="to"
    class="app-card-link"
  >
    <Card class="app-card">
      <template
        v-if="$slots.title"
        #title
      >
        <slot name="title" />
      </template>
      <template
        v-if="$slots.subtitle"
        #subtitle
      >
        <slot name="subtitle" />
      </template>
      <template
        v-if="$slots.content"
        #content
      >
        <slot name="content" />
      </template>
      <slot />
      <template
        v-if="$slots.footer"
        #footer
      >
        <slot name="footer" />
      </template>
    </Card>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { RouteLocationRaw } from 'vue-router';

defineProps<{
  to: RouteLocationRaw;
}>();
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.app-card-link {
  @include utilities.reset-link;

  // Add smooth transition for hover effects
  .app-card {
    transition: all 0.2s ease-in-out;
  }

  // Hover state using PrimeVue theme variables
  &:hover .app-card {
    background: var(--p-content-hover-background);
    border-color: var(--p-content-hover-border-color);
  }

  // Focus state for accessibility
  &:focus {
    outline: none;

    .app-card {
      outline: 2px solid var(--p-focus-ring-color);
      outline-offset: 2px;
    }
  }

  // Active state
  &:active .app-card {
    transform: scale(0.99);
  }
}
</style>
