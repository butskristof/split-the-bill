<template>
  <div class="index-page">
    <div>Hello, world!</div>
    <div>
      <ColorModeButton />
    </div>
    <div v-if="true">
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
      <div>hey</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import ColorModeButton from '~/components/app/ColorModeButton.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';
.index-page {
  @include utilities.flex-column;
  flex-grow: 1;
  margin: var(--default-spacing);
  padding: var(--default-spacing);
  border-radius: var(--default-radius);

  background-color: var(--p-surface-0);
  border: 1px solid var(--p-surface-200);
  @include utilities.dark-mode {
    background-color: var(--p-surface-900);
    border: 1px solid var(--p-surface-700);
  }
}
</style>
