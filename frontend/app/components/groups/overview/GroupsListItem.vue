<template>
  <AppLinkCard
    :to="{ name: 'groups-id', params: { id: group.id } }"
    class="groups-list-item"
  >
    <template #title>
      <div class="group-name">{{ group.name }}</div>
      <div class="member-count"><i class="pi pi-users" /> {{ group.memberCount }}</div>
    </template>
    <template #footer>
      <div class="actions">
        <i class="pi pi-arrow-right" />
      </div>
    </template>
  </AppLinkCard>
</template>

<script setup lang="ts">
import AppLinkCard from '~/components/common/AppLinkCard.vue';
import type { components } from '#open-fetch-schemas/backend-api';
type Group = components['schemas']['GetGroups.Response.GroupDto'];

defineProps<{
  group: Group;
}>();
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list-item {
  ::v-deep(.p-card-title) {
    @include utilities.flex-row-justify-between-align-center;

    .member-count {
      font-weight: initial;
      font-size: initial;
    }
  }
  ::v-deep(.p-card-footer) {
    @include utilities.flex-row-justify-end;
  }
}
</style>
