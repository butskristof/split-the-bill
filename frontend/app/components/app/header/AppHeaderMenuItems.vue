<template>
  <div class="menu-items">
    <Button
      v-for="(item, i) in menuItems"
      :key="item.name"
      :label="item.name"
      :icon="item.icon"
      :variant="i === active ? '' : 'text'"
      severity="secondary"
      size="small"
    />
  </div>
</template>

<script setup lang="ts">
const active = 0;

const menuItems = [
  {
    name: 'Groups',
    icon: 'pi pi-users',
  },
  {
    name: 'Settings',
    icon: 'pi pi-cog',
  },
];
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.menu-items {
  @include utilities.flex-column(false);
  gap: calc(var(--default-spacing) / 2);

  @include utilities.media-min-lg {
    @include utilities.flex-row(false);
  }

  ::v-deep(.p-button) {
    justify-content: flex-start;
  }
}
</style>
