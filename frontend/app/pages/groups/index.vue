<template>
  <div class="groups-page">
    <AppPageMain class="main">
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
    </AppPageMain>
  </div>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import GroupsList from '~/components/groups/overview/list/GroupsList.vue';
import CreateGroup from '~/components/groups/edit/CreateGroup.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';

const query = await useLazyBackendApi('/Groups', { key: 'groups' });
const isLoading = computed(() => query.status.value === 'pending');

const showCreateGroup = ref(false);
const closeCreateGroup = () => {
  showCreateGroup.value = false;
  query.refresh();
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.main {
  @include utilities.flex-column;
}

.header {
  @include utilities.flex-row-justify-between-align-center;

  .actions {
    @include utilities.flex-row-align-center;
  }
}
</style>
