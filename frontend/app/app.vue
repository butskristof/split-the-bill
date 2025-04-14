<template>
  <UApp>
    <NuxtRouteAnnouncer />

    <div class="app">
      <AppHeader />

      <UContainer class="content-container">
        <main>
          <NuxtPage />
        </main>

        <USeparator
          icon="i-mynaui-envelope"
          color="primary"
        />

        <AppFooter class="footer" />
      </UContainer>
    </div>
  </UApp>
</template>

<script setup lang="ts">
import AppHeader from '~/components/app/AppHeader.vue';
import AppFooter from '~/components/app/AppFooter.vue';

// UApp provides global configuration (Reka UI's "ConfigProvider"), sets up the providers for
// toasts, tooltips, scroll behaviour, ...
// NuxtRouteAnnouncer announces route changes to screen readers

// the actual app as far as the UI goes is wrapped in an 'app' class: it should feature a sticky
// header, growing page content and a footer which is always at the bottom, even if the content
// doesn't fill the page

useHead({
  // prepend the page's title if defined, fall back to "Split the bill" otherwise
  titleTemplate: (title) => (title ? `${title} - ` : '') + 'Split the bill',
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.app {
  min-height: 100vh;
  @include utilities.flex-column(false);

  .content-container {
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
