<template>
  <footer>
    <AppFooterTitle />
    <div class="icon-row">
      <ColorModeButtonHeadless v-slot="{ icon, toggleColorMode, nextMode }">
        <button
          v-tooltip.top="`Switch to ${nextMode} mode`"
          aria-label="Toggle color mode"
          @click="toggleColorMode"
        >
          <i :class="icon" />
        </button>
      </ColorModeButtonHeadless>
      <a
        v-tooltip.top="'GitHub'"
        href="https://github.com/butskristof/split-the-bill"
        target="_blank"
        rel="noopener"
      >
        <i
          class="pi pi-github"
          aria-label="GitHub"
        />
      </a>
    </div>
  </footer>
</template>

<script setup lang="ts">
import AppFooterTitle from '~/components/app/footer/AppFooterTitle.vue';
import ColorModeButtonHeadless from '~/components/app/ColorModeButtonHeadless.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

footer {
  @include utilities.app-container;
  & {
    @include utilities.flex-row-justify-between-align-center;
    margin-bottom: calc(var(--default-spacing) * 2);
    padding-inline: var(--default-spacing);
  }

  .icon-row {
    @include utilities.flex-row-align-center;

    button {
      all: unset;

      &:focus {
        outline: revert;
      }
    }

    button,
    a {
      cursor: pointer;
      font-size: var(--text-lg);
      @include utilities.icon-color;
    }
  }

  .link {
    @include utilities.reset-link(false);
  }
}
</style>
