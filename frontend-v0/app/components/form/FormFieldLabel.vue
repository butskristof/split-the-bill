<template>
  <label :for="htmlFor">
    <slot />
    <template v-if="required">
      <FormFieldLabelRequiredIndicator />
    </template>
  </label>
</template>

<script setup lang="ts">
import FormFieldLabelRequiredIndicator from '~/components/form/FormFieldLabelRequiredIndicator.vue';

defineProps<{
  htmlFor: string;
  required: boolean;
}>();
</script>

<style scoped lang="scss">
@use '@/assets/styles/utilities.scss';

label {
  font-weight: var(--font-weight-medium);
}
</style>
