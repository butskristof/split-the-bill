<template>
  <div class="groups-page">
    <AppPageMain class="main">
      <div class="header">
        <h1>Groups</h1>
        <div class="actions">
          <Button
            label="Create group"
            icon="pi pi-plus"
            @click="showCreateGroup = true"
          />
        </div>
      </div>
      <GroupsList :refresh-key="refreshKey" />
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

const refreshKey = ref(0);
const showCreateGroup = ref(false);
const closeCreateGroup = () => {
  showCreateGroup.value = false;
  ++refreshKey.value; // Trigger refresh of groups list
};
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
