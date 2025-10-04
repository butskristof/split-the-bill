<template>
  <div>
    <NuxtRouteAnnouncer />
    <div class="app-layout">
      <AppHeader />
      <div class="app-body">
        <div class="app-main">
          <NuxtPage />
        </div>
        <AppFooter />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import AppHeader from '~/components/app/AppHeader.vue';
import AppFooter from '~/components/app/AppFooter.vue';
useHead({
  htmlAttrs: {
    class: 'dark-mode',
  },
});
</script>

<style scoped lang="scss">
@use '~/styles/utilities';

.app-layout {
  min-height: 100vh;
  @include utilities.flex-column(false);

  background-color: var(--p-surface-50);
  @include utilities.dark-mode {
    background-color: var(--p-surface-950);
  }

  .app-body {
    // make sure the content container fills at least the available screen space below the header
    min-height: calc(100vh - var(--app-header-height));
    @include utilities.flex-column(false);

    .app-main {
      @include utilities.app-container;

      & {
        flex-grow: 1;
        margin: var(--default-spacing);

        @include utilities.media-min-lg {
          margin-inline: auto;
        }
      }
    }
  }
}
</style>
