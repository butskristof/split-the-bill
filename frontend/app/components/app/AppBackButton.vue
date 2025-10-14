<template>
  <NuxtLink :to="destination">
    <Button
      class="back-button"
      :label="label"
      icon="pi pi-arrow-left"
      variant="text"
    />
  </NuxtLink>
</template>

<script setup lang="ts">
import type { RouteLocationRaw } from 'vue-router';

const props = withDefaults(
  defineProps<{
    label?: string | undefined;
    to?: RouteLocationRaw | undefined;
    prioritizePreviousRoute?: boolean;
  }>(),
  {
    label: 'Back',
    to: undefined,
    prioritizePreviousRoute: false,
  },
);

const router = useRouter();
// null for non-internal routes
const previousRoute = computed<string | null>(() => router.options.history.state.back as string);

const destination = computed<RouteLocationRaw | string>(() => {
  if (props.prioritizePreviousRoute && previousRoute.value) return previousRoute.value;
  return props.to ?? previousRoute.value ?? '/';
});
</script>

<style scoped lang="scss">
.back-button {
  --p-button-padding-x: 0;
  --p-button-padding-y: 0;

  &:hover,
  &:focus,
  &:active {
    --p-button-padding-x: 0.75rem;
    --p-button-padding-y: 0.5rem;
    margin: -0.5rem -0.75rem;
  }
}
</style>
