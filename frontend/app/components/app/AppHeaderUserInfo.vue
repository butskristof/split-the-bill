<template>
  <div class="user-info">
    <Avatar
      shape="circle"
      label="K"
    />
    <div class="user-name">Kristof Buts</div>
  </div>
</template>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.user-info {
  @include utilities.flex-row-align-center(false);
  gap: calc(var(--default-spacing) / 2);

  .user-name {
    color: var(--p-surface-800);
    font-weight: var(--font-weight-medium);

    @include utilities.dark-mode {
      color: var(--p-surface-100);
    }
  }
}
</style>
