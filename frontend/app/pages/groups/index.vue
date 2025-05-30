<template>
  <div class="groups-page">
    <AppPageMain class="main">
      <div class="header">
        <h1>Groups</h1>
        <div class="actions">
          <Button
            label="Create group"
            icon="pi pi-plus"
          />
        </div>
      </div>
      <GroupsList />
    </AppPageMain>
  </div>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import GroupsList from '~/components/groups/overview/list/GroupsList.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.main {
  @include utilities.flex-column;
}

.header {
  @include utilities.flex-row-justify-between-align-center;
}
</style>
