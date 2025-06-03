<template>
  <div class="app">
    <AppHeader />
    <div class="content">
      <div class="page">
        <NuxtPage />
      </div>

      <AppFooter />
    </div>
  </div>
</template>

<script setup lang="ts">
import AppHeader from '~/components/app/AppHeader.vue';
import AppFooter from '~/components/app/AppFooter.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.app {
  min-height: 100vh;
  @include utilities.flex-column(false);

  background-color: var(--p-surface-50);
  @include utilities.dark-mode {
    background-color: var(--p-surface-950);
  }

  .content {
    // make sure the content container fills at least the available screen space below the header
    min-height: calc(100vh - var(--app-header-height));
    @include utilities.flex-column(false);

    .page {
      @include utilities.app-container;

      & {
        flex-grow: 1;
        margin: var(--default-spacing);

        @include utilities.media-min-xl {
          margin-inline: auto;
          width: 100%;
        }
      }
    }
  }
}
</style>
