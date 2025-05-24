<template>
  <div class="groups-page">
    <AppPageMain class="main">
      <h1>Groups</h1>
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
</style>
