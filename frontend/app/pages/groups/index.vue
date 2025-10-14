<template>
  <div class="page-groups">
    <div class="page-header">
      <h1>Your groups</h1>
      <div class="actions">
        <Button
          label="Create new group"
          icon="pi pi-plus"
          @click="openCreateGroupDialog"
        />
      </div>
    </div>
    <GroupsList
      v-if="groups"
      :groups="groups"
    />
    <EditGroupDialog
      v-if="showCreateGroupDialog"
      @close="closeCreateGroupDialog"
    />
  </div>
</template>

<script setup lang="ts">
import GroupsList from '~/components/groups/overview/GroupsList.vue';
import EditGroupDialog from '~/components/groups/overview/EditGroupDialog.vue';
import { useGetGroupsQuery } from '~/composables/backend-api/useGetGroupsQuery';

// TODO loading state
const { data: groups, suspense } = useGetGroupsQuery();
await suspense();

//#region create group dialog

const showCreateGroupDialog = ref(false);
const openCreateGroupDialog = () => (showCreateGroupDialog.value = true);
const closeCreateGroupDialog = () => (showCreateGroupDialog.value = false);

//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.page-groups {
  @include utilities.flex-column;
}

.page-header {
  @include utilities.flex-row-justify-between-align-center;
  flex-wrap: wrap;

  .actions {
    //flex-wrap: nowrap;
    margin-left: auto;
  }
}
</style>
