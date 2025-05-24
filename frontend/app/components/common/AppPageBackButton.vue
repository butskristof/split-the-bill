<template>
  <Button
    class="back-button"
    variant="text"
    severity="secondary"
    label="Back to overview"
    icon="pi pi-arrow-left"
  />
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.back-button {
  margin-top: calc(var(--default-spacing) * -1);

  @include utilities.media-max-lg {
    margin-left: calc(var(--default-spacing) * -0.5);
  }
}
</style>
