<template>
  <AppCard
    tag="main"
    class="groups-overview"
  >
    <div class="header">
      <h1>Groups</h1>
      <div class="actions">
        <LoadingIndicator v-if="isLoading" />
        <Button
          label="Create group"
          icon="pi pi-plus"
          @click="showCreateGroup = true"
        />
      </div>
    </div>
    <GroupsList :query="query" />
    <CreateGroup
      v-if="showCreateGroup"
      @close="closeCreateGroup"
    />
  </AppCard>
</template>

<script setup lang="ts">
import GroupsList from '~/components/groups/overview/list/GroupsList.vue';
import CreateGroup from '~/components/groups/edit/CreateGroup.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import AppCard from '~/components/app/AppCard.vue';

const query = await useLazyBackendApi('/Groups', { key: 'groups' });
const isLoading = computed(() => query.status.value === 'pending');

const showCreateGroup = ref(false);
const closeCreateGroup = (created: boolean) => {
  showCreateGroup.value = false;
  if (created) query.refresh();
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
