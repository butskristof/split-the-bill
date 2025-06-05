<template>
  <div class="actions">
    <Button
      v-for="action in actions"
      :key="action.name"
      :icon="action.icon"
      :aria-label="action.name"
      severity="secondary"
      variant="text"
      size="small"
    />
  </div>
</template>

<script setup lang="ts">
const actions = [
  {
    name: 'Notifications',
    icon: 'pi pi-bell',
  },
  // {
  //   name: 'Settings',
  //   icon: 'pi pi-cog',
  // },
];
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.actions {
  @include utilities.flex-row;
}
</style>
