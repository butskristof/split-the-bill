<template>
  <NuxtLink :to="to">
    <Button
      class="back-button"
      label="Back to overview"
      icon="pi pi-arrow-left"
      variant="text"
      severity="secondary"
    />
  </NuxtLink>
</template>

<script setup lang="ts">
import type { RouteLocationRaw } from '#vue-router';

const props = defineProps<{
  defaultRoute?: RouteLocationRaw;
}>();
const router = useRouter();
const previousRoute = computed(() => router.options.history.state.back);

const to = computed<RouteLocationRaw>(() => {
  if (previousRoute.value) return { path: previousRoute.value as string };
  if (props.defaultRoute) return props.defaultRoute;
  return { name: 'index' };
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.back-button {
  margin-top: calc(var(--default-spacing) * -1);
  margin-left: calc(var(--default-spacing) * -0.5);

  &:hover,
  &:active,
  &:focus {
    background: transparent !important;
    color: var(--p-surface-600) !important;
  }

  &:active,
  &:focus {
    color: var(--p-surface-800) !important;
  }
}
</style>
