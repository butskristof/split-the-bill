<template>
  <footer class="app-footer">
    <div class="app-name">Split the bill</div>
    <div class="links">GitHub</div>
  </footer>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '@/styles/_utilities.scss';

footer {
  @include utilities.app-container;
  @include utilities.flex-row-justify-between;
  padding: var(--default-spacing);
}
</style>
