<template>
  <UCard
    variant="subtle"
    class="card"
  >
    <template #header>
      <div class="header">
        <UIcon
          name="i-mynaui-danger-square"
          class="icon"
        />
        Something went wrong
      </div>
    </template>
    <div>
      We're having trouble loading this content. <br />
      Please review the details below or try again.

      <div class="actions">
        <UButton
          :icon="showDetails ? 'i-mynaui-eye-slash' : 'i-mynaui-eye'"
          @click="toggleShowDetails"
        >
          {{ showDetails ? 'Hide' : 'Show' }} technical details</UButton
        >
      </div>
      <pre v-if="showDetails">{{ JSON.stringify(error, null, 2) }}</pre>
    </div>
  </UCard>
</template>

<script setup lang="ts">
import type { ProblemDetails } from '#shared/api-clients/split-the-bill-api/types';

defineProps<{
  error: string | ProblemDetails;
}>();

const showDetails = ref(false);
const toggleShowDetails = () => {
  showDetails.value = !showDetails.value;
};
</script>

<style scoped>
@reference '~/assets/css/main.css';

.card {
  @apply bg-red-50 dark:bg-red-900/50;
  @apply ring-red-200 dark:ring-red-700/75;
  @apply text-red-700 dark:text-red-200;
}

.header {
  @apply font-semibold;
  display: flex;
  align-items: center;
  font-size: 110%;

  .icon {
    @apply size-6;
    margin-right: 0.5rem;
  }
}

.actions {
  margin-top: 1rem;
}

/*.error-container {*/
/*  @apply border;*/
/*  @apply border-red-200;*/
/*  @apply bg-red-50;*/
/*}*/

/*.error-icon {*/
/*  @apply w-8 h-8 text-red-500;*/
/*}*/

/*.error-title {*/
/*  @apply text-lg font-semibold text-red-800;*/
/*}*/

/*.error-message {*/
/*  @apply text-sm text-red-700;*/
/*}*/

/*.error-actions {*/
/*  @apply pt-2;*/
/*}*/

/*.error-details {*/
/*  @apply mt-4 p-3 bg-white rounded border border-gray-200;*/
/*}*/
</style>
