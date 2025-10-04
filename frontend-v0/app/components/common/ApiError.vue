<template>
  <Card class="card">
    <template #title>
      <div class="title">
        <Icon
          name="prime:exclamation-circle"
          class="icon"
        />
        Something went wrong
      </div>
    </template>
    <template #content>
      <div>
        We're having trouble loading this content. <br />
        Please review the details below or try again.
      </div>

      <div class="actions">
        <Button
          :label="`${showDetails ? 'Hide' : 'Show'} technical details`"
          :icon="`pi pi-${showDetails ? 'eye-slash' : 'eye'}`"
          severity="danger"
          @click="toggleShowDetails"
        />
      </div>

      <PreformattedText
        v-if="showDetails"
        :value="error"
      />
    </template>
  </Card>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import type { ProblemDetails } from '#shared/types';

defineProps<{
  error: string | ProblemDetails;
}>();

const showDetails = ref(false);
const toggleShowDetails = () => (showDetails.value = !showDetails.value);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.title {
  @include utilities.flex-row-align-center(false);
  gap: 0.5rem;

  @include utilities.bold;

  .icon {
    margin-top: -1px;
    font-size: 125%;
  }
}

.card {
  background-color: var(--p-rose-100);
  border: 1px solid var(--p-rose-300);
  @include utilities.dark-mode {
    // https://stackoverflow.com/a/71098929
    background-color: rgba(from var(--p-rose-300) r g b / 30%);
    border: 1px solid var(--p-rose-500);
  }
}

::v-deep(.p-card-content) {
  @include utilities.flex-column;
}
</style>
