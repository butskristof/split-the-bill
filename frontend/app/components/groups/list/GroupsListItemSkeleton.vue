<template>
  <div class="group-list-item">
    <USkeleton class="icon h-10 w-10 rounded-full" />
    <USkeleton class="name h-5 w-50" />
  </div>
</template>

<script setup lang="ts">
// TODO make name width dynamic
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.group-list-item {
  @include utilities.flex-row-align-center;
  padding: var(--default-spacing);
  border-radius: var(--ui-radius);
  @include utilities.list-item-hover;
  @include utilities.list-item-separator;

  &:hover {
    .icon,
    .name {
      background-color: var(--ui-bg-accented);
    }
  }
}
</style>
