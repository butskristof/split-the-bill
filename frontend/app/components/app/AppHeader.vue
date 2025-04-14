<template>
  <header class="backdrop-blur bg-(--ui-bg)/75">
    <UContainer class="header-content">
      <AppHeaderTitle />

      <div class="header-actions">
        <UAvatar />
      </div>
    </UContainer>
  </header>
</template>

<script setup lang="ts">
import AppHeaderTitle from './AppHeaderTitle.vue';
// falling back to setting the backdrop and bg classes in the markup since they don't work with
// apply and can't easily be replicated in plain (S)CSS
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

header {
  // header always has a predefined height and should always still to the top
  height: var(--ui-header-height);
  position: sticky;
  top: 0;
  z-index: 1;
  border-bottom: 1px solid var(--ui-border);

  // vertically center the content inside the header
  @include utilities.flex-row-align-center;

  .header-content {
    @include utilities.flex-row-justify-between-align-center;
  }
}
</style>
