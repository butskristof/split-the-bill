<template>
  <UCard
    variant="subtle"
    class="card"
  >
    <template #header>
      <div class="header">
        <UIcon
          name="i-mynaui-danger-square"
          class="icon size-6"
        />
        Something went wrong
      </div>
    </template>
    <div class="content">
      <div class="lead">
        We're having trouble loading this content. <br />
        Please review the details below or try again.
      </div>

      <div class="actions">
        <UButton
          :icon="showDetails ? 'i-mynaui-eye-slash' : 'i-mynaui-eye'"
          @click="toggleShowDetails"
        >
          {{ showDetails ? 'Hide' : 'Show' }} technical details</UButton
        >
      </div>

      <PreformattedText
        v-if="showDetails"
        :value="error"
      />
    </div>
  </UCard>
</template>

<script setup lang="ts">
import type { ProblemDetails } from '#shared/api-clients/split-the-bill-api/types';
import PreformattedText from '~/components/common/PreformattedText.vue';

defineProps<{
  error: string | ProblemDetails;
}>();

const showDetails = ref(false);
const toggleShowDetails = () => {
  showDetails.value = !showDetails.value;
};
</script>

<style scoped>
@reference '~/assets/styles/main.css';

.card {
  @apply bg-red-50 dark:bg-red-900/50;
  @apply ring-red-200 dark:ring-red-700/75;
  @apply text-red-700 dark:text-red-200;
  border-radius: var(--ui-radius);
}
</style>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.header {
  @include utilities.semibold;
  @include utilities.flex-row-align-center;
  font-size: 110%;

  .icon {
    margin-right: 0.5rem;
  }
}

.content {
  @include utilities.flex-column;
}
</style>
