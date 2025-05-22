<template>
  <NuxtRouteAnnouncer />
  <div class="app">
    <AppHeader />
    <div class="main-container">
      <main>
        <h1>Hello, world!</h1>
        <ColorModeButton />
      </main>

      <AppFooter />
    </div>
  </div>
</template>

<script setup lang="ts">
import ColorModeButton from '~/components/app/ColorModeButton.vue';
import AppHeader from '~/components/app/AppHeader.vue';
import AppFooter from '~/components/app/AppFooter.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.app {
  min-height: 100vh;
  @include utilities.flex-column(false);

  font-family: 'Nunito', sans-serif;
  background-color: var(--p-surface-50);
  @include utilities.dark-mode {
    background-color: var(--p-surface-950);
  }

  .main-container {
    // make sure the content container fills at least the available screen space below the header
    min-height: calc(100vh - var(--ui-header-height));
    @include utilities.flex-column(false);

    main {
      margin-top: var(--default-spacing);
      margin-bottom: var(--default-spacing);
      flex-grow: 1;
    }
  }
}
</style>
