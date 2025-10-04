<template>
  <div class="groups-list">
    <GroupsListItem
      v-for="group in data?.groups"
      :key="group.id"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import GroupsListItem from '~/components/groups/overview/GroupsListItem.vue';

const { data } = useLazyBackendApi('/Groups');
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list {
  @include utilities.flex-column;
}
</style>
