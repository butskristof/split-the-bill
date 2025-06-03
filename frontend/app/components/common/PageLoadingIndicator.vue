<template>
  <div class="loading-indicator">
    <LoadingIndicator class="indicator" />
    <div v-if="message">{{ message }}</div>
    <div v-if="entity">Loading {{ entity }}</div>
  </div>
</template>

<script setup lang="ts">
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';

defineProps<{
  message?: string;
  entity?: string;
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.loading-indicator {
  padding-block: calc(var(--default-spacing) * 4);
  text-align: center;
  @include utilities.flex-column(false);
  align-items: center;
  justify-content: center;
  gap: 1rem;

  .indicator {
    font-size: 2rem;
  }
}
</style>
