<template>
  <header>
    <div>left</div>
    <div>right</div>
  </header>
</template>

<script setup lang="ts"></script>
<style scoped lang="scss">
@use '~/assets/styles/utilities';

header {
  // header always has a predefined height and should always still to the top
  height: var(--app-header-height);
  position: sticky;
  top: 0;
  z-index: 1;

  // vertically center the content inside the header
  @include utilities.flex-row-justify-between-align-center;
  padding-inline: var(--default-spacing);

  background-color: var(--p-surface-0);
  border-bottom: 1px solid var(--p-surface-200);

  @include utilities.dark-mode {
    background-color: var(--p-surface-900);
    border-bottom: 1px solid var(--p-surface-800);
  }
}
</style>
