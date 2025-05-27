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
  @include utilities.flex-row-align-center;
  gap: 0.5rem;

  font-size: var(--text-2xl);
  @include utilities.bold;
  @include utilities.reset-link;

  .icon {
    font-size: 125%;
  }
}
</style>
