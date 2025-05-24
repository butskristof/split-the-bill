<template>
  <main>
    <slot />
  </main>
</template>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

main {
  width: auto;
  flex-grow: 1;
  padding: var(--default-spacing);
  border-radius: var(--default-radius);

  @include utilities.flex-column;

  background-color: var(--p-surface-0);
  border: 1px solid var(--p-surface-200);
  @include utilities.dark-mode {
    background-color: var(--p-surface-900);
    border: 1px solid var(--p-surface-700);
  }
}
</style>
