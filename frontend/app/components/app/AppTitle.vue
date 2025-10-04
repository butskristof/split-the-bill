<template>
  <NuxtLink
    to="/"
    class="app-title-link"
  >
    <div class="app-title">
      <span class="title">Split the bill</span>
    </div>
  </NuxtLink>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.app-title-link {
  @include utilities.reset-link;
}

.app-title {
  @include utilities.flex-row;
  gap: calc(var(--default-spacing) / 2);
  font-size: var(--text-xl);
  font-weight: var(--font-weight-semibold);
}
</style>
