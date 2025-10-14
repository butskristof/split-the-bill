<template>
  <div
    v-if="group"
    class="group-detail"
  >
    <div class="page-header">
      <h1>{{ group.name }}</h1>
    </div>
    <div class="actions">
      <Button
        label="Add expense"
        icon="pi pi-plus"
        @click="openCreateExpenseDialog"
      />
      <EditExpenseDialog
        v-if="showCreateExpenseDialog"
        :group-id="group.id!"
        :members="group.members!.map((m) => ({ id: m.id!, name: m.name! }))"
        @close="closeCreateExpenseDialog"
      />
      <Button
        label="Add payment"
        icon="pi pi-plus"
        @click="openCreatePaymentDialog"
      />
      <EditPaymentDialog
        v-if="showCreatePaymentDialog"
        :group-id="group.id!"
        :members="group.members!.map((m) => ({ id: m.id!, name: m.name! }))"
        @close="closeCreatePaymentDialog"
      />
    </div>
    <RecentActivity :group="group" />
    <GroupDetailMembers :group="group" />
    <div class="edit-delete-group">
      <div class="edit">
        <Button
          label="Edit group details"
          icon="pi pi-pen-to-square"
          variant="text"
          @click="openEditGroupDialog"
        />
        <EditGroupDialog
          v-if="showEditGroupDialog"
          :group="{ id: group.id!, name: group.name! }"
          @close="closeEditGroupDialog"
        />
      </div>
      <div class="delete">
        <Button
          label="Delete group"
          icon="pi pi-trash"
          severity="danger"
          variant="text"
          @click="openDeleteGroupDialog"
        />
        <DeleteGroupDialog
          v-if="showDeleteGroupDialog"
          :group="{ id: group.id!, name: group.name! }"
          @close="closeDeleteGroupDialog"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import EditExpenseDialog from '~/components/groups/detail/EditExpenseDialog.vue';
import EditPaymentDialog from '~/components/groups/detail/EditPaymentDialog.vue';
import type { Group } from '#shared/types/api';
import DeleteGroupDialog from '~/components/groups/detail/DeleteGroupDialog.vue';
import RecentActivity from '~/components/groups/detail/RecentActivity.vue';
import GroupDetailMembers from '~/components/groups/detail/GroupDetailMembers.vue';
import EditGroupDialog from '~/components/groups/overview/EditGroupDialog.vue';

defineProps<{
  group: Group;
}>();

//#region create expense dialog
const showCreateExpenseDialog = ref(false);
const openCreateExpenseDialog = () => (showCreateExpenseDialog.value = true);
const closeCreateExpenseDialog = () => (showCreateExpenseDialog.value = false);
//#endregion

//#region create payment dialog
const showCreatePaymentDialog = ref(false);
const openCreatePaymentDialog = () => (showCreatePaymentDialog.value = true);
const closeCreatePaymentDialog = () => (showCreatePaymentDialog.value = false);
//#endregion

//#region edit group dialog
const showEditGroupDialog = ref(false);
const openEditGroupDialog = () => (showEditGroupDialog.value = true);
const closeEditGroupDialog = () => (showEditGroupDialog.value = false);
//#endregion

//#region delete group dialog
const showDeleteGroupDialog = ref(false);
const openDeleteGroupDialog = () => (showDeleteGroupDialog.value = true);
const closeDeleteGroupDialog = () => (showDeleteGroupDialog.value = false);
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-detail {
  @include utilities.flex-column;

  .actions {
    @include utilities.flex-row;
  }

  .edit-delete-group {
    margin-top: calc(var(--default-spacing));
    @include utilities.flex-row-justify-between-align-center;
    flex-wrap: wrap;
  }
}
</style>
