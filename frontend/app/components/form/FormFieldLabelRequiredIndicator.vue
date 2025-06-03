<template>
  <span>*</span>
</template>

<style scoped lang="scss">
@use '@/assets/styles/utilities.scss';

span {
  // same as message w/ severity error
  color: var(--p-red-600);
  @include utilities.dark-mode {
    color: var(--p-red-500);
  }
}
</style>
