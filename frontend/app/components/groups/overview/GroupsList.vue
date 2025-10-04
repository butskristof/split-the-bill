<template>
  <div class="groups-list">
    <GroupsListItem
      v-for="group in groups"
      :key="group.id"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import GroupsListItem from '~/components/groups/overview/GroupsListItem.vue';
import type { components } from '#open-fetch-schemas/backend-api';

type Groups = components['schemas']['GetGroups.Response.GroupDto'][];

defineProps<{
  groups: Groups;
}>();
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list {
  @include utilities.flex-column;
}
</style>
