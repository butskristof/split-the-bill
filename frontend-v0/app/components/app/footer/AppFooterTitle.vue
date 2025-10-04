<template>
  <NuxtLink
    to="/"
    class="app-title"
  >
    <Icon
      name="mynaui:envelope"
      class="icon"
    />
    Split the bill
  </NuxtLink>
</template>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.app-title {
  @include utilities.flex-row-align-center(false);
  gap: 0.5rem;

  //font-size: var(--text-md);
  @include utilities.bold;
  @include utilities.reset-link;

  .icon {
    margin-top: -1px;
    font-size: 125%;
  }
}
</style>
