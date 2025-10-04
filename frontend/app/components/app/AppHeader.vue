<template>
  <div class="app-header">
    <header>
      <div class="app-title">Split the bill</div>
      <div class="actions">actions</div>
    </header>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/styles/utilities';

.app-header {
  // header always has a predefined height and should always still to the top
  height: var(--app-header-height);
  position: sticky;
  top: 0;
  z-index: 1;

  //background-color: var(--p-surface-0);
  background-color: hsl(from var(--p-surface-50) h s l / 0.5);
  backdrop-filter: blur(4px);
  border-bottom: 1px solid var(--p-surface-200);
  @include utilities.dark-mode {
    //background-color: var(--p-surface-950);
    background-color: hsl(from var(--p-surface-950) h s l / 0.5);
  }
}

header {
  @include utilities.app-container;
  height: 100%;
  @include utilities.flex-row-justify-between-align-center;
  gap: calc(var(--default-spacing) * 2);
}
</style>
