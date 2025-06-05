<template>
  <AppCard
    tag="main"
    class="groups-overview"
  >
    <div class="header">
      <h1>Groups</h1>
      <div class="actions">
        <LoadingIndicator v-if="isLoading" />
        <CreateGroup @created="onCreated" />
      </div>
    </div>
    <GroupsList :query="query" />
  </AppCard>
</template>

<script setup lang="ts">
import GroupsList from '~/components/groups/overview/list/GroupsList.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import AppCard from '~/components/common/AppCard.vue';
import CreateGroup from '~/components/groups/edit/CreateGroup.vue';
import type { Query } from '~/types';
import type { GetGroupsResponse } from '~/components/groups/overview/types';

const query: Query<GetGroupsResponse> = await useLazyBackendApi('/Groups', { key: 'groups' });
const isLoading = computed<boolean>(() => query.status.value === 'pending');

const onCreated = (): void => {
  query.refresh();
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.groups-overview {
  @include utilities.flex-column;

  .header {
    @include utilities.flex-row-justify-between-align-center;

    .actions {
      @include utilities.flex-row-align-center;
    }
  }
}
</style>
