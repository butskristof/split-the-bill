<template>
  <footer>
    <AppFooterTitle />
    <div class="right">
      <Button
        v-for="link in links"
        :key="link.name"
        class="link"
        :aria-label="link.name"
        :icon="link.icon"
        variant="text"
        severity="secondary"
        :href="link.to"
        as="a"
        target="_blank"
        rel="noopener"
      />
    </div>
  </footer>
</template>

<script setup lang="ts">
import AppFooterTitle from '~/components/app/AppFooterTitle.vue';

const links = [
  {
    name: 'GitHub',
    icon: 'pi pi-github',
    to: 'https://github.com/butskristof/split-the-bill',
  },
];
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

footer {
  @include utilities.flex-row-justify-between-align-center;

  margin-top: var(--default-spacing);
  margin-bottom: calc(var(--default-spacing) * 2);
  margin-inline: var(--default-spacing);

  .link {
    @include utilities.reset-link(false);
  }
}
</style>
