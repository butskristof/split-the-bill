<template>
  <footer>
    <div>
      <p>&copy; Split the bill 2025</p>
    </div>

    <div>
      <ColorModeButton />
      <UButton
        to="https://github.com/butskristof/split-the-bill"
        target="_blank"
        icon="i-mynaui-brand-github"
        aria-label="GitHub"
        color="neutral"
        variant="ghost"
      />
    </div>
  </footer>
</template>

<script setup lang="ts">
import ColorModeButton from '~/components/app/ColorModeButton.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

footer {
  @include utilities.flex-row-justify-between-align-center;

  margin-top: var(--default-spacing);
  margin-bottom: calc(var(--default-spacing) * 2);
}
</style>
