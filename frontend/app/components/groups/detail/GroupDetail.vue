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
      <CreateExpenseDialog
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
      <CreatePaymentDialog
        v-if="showCreatePaymentDialog"
        :group-id="group.id!"
        :members="group.members!.map((m) => ({ id: m.id!, name: m.name! }))"
        @close="closeCreatePaymentDialog"
      />
    </div>
    <PreformattedText :value="group" />
    <div class="delete-group">
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
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import CreateExpenseDialog from '~/components/groups/detail/CreateExpenseDialog.vue';
import CreatePaymentDialog from '~/components/groups/detail/CreatePaymentDialog.vue';
import type { Group } from '#shared/types/api';
import DeleteGroupDialog from '~/components/groups/detail/DeleteGroupDialog.vue';

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

  .delete-group {
    margin-left: auto;
  }
}
</style>
