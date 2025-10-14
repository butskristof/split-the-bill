<template>
  <footer class="app-footer">
    <div class="app-name" />
    <div class="links">
      <AppColorModeToggle />
      <a
        v-tippy="'GitHub'"
        href="https://github.com/butskristof/split-the-bill"
        target="_blank"
        rel="noopener"
      >
        <i class="pi pi-github" />
      </a>
    </div>
  </footer>
</template>

<script setup lang="ts">
import AppColorModeToggle from '~/components/app/AppColorModeToggle.vue';
</script>

<style scoped lang="scss">
@use '@/styles/_utilities.scss';

footer {
  @include utilities.app-container;
  @include utilities.flex-row-justify-between;
  padding: var(--default-spacing);

  .links {
    @include utilities.flex-row-align-center;

    a {
      @include utilities.reset-link;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }
}
</style>
