<template>
  <div class="header">
    <h2>Recent activity</h2>
    <AddActivityDropdown />
  </div>
  <div class="actions">
    <UButton
      trailing-icon="i-mynaui-arrow-right"
      variant="ghost"
      label="Show all activity"
    />
  </div>
</template>

<script setup lang="ts">
import AddActivityDropdown from '~/components/groups/activities/AddActivityDropdown.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.header {
  @include utilities.flex-row-justify-between;
}

.actions {
  @include utilities.flex-row;
  justify-content: flex-end;
}
</style>
