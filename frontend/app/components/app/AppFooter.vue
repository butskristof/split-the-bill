<template>
  <footer>footer</footer>
</template>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

footer {
  @include utilities.flex-row-justify-between-align-center;

  margin-top: var(--default-spacing);
  margin-bottom: calc(var(--default-spacing) * 2);
}
</style>
