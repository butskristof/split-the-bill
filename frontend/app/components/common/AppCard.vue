<template>
  <component
    :is="tag"
    class="app-card"
  >
    <slot />
  </component>
</template>

<script setup lang="ts">
// typed props w/ default value
// https://vuejs.org/guide/typescript/composition-api.html#props-default-values
interface Props {
  tag?: string;
}

const { tag = 'div' } = defineProps<Props>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.app-card {
  @include utilities.app-card;
}
</style>
