<template>
  <div class="groups-list">
    <GroupsListItem />
    <GroupsListItem />
    <GroupsListItem />
  </div>
</template>

<script setup lang="ts">
import GroupsListItem from '~/components/groups/overview/GroupsListItem.vue';
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list {
  @include utilities.flex-column;
}
</style>
