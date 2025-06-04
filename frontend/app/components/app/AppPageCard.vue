<template>
  <div class="app-card">
    <slot />
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.app-card {
  @include utilities.app-card;
}
</style>
