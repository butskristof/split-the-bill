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
    </div>
    <PreformattedText :value="group" />
  </div>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import CreateExpenseDialog from '~/components/groups/detail/CreateExpenseDialog.vue';
import type { Group } from '#shared/types/api';

defineProps<{
  group: Group;
}>();

//#region create expense dialog
const showCreateExpenseDialog = ref(false);
const openCreateExpenseDialog = () => (showCreateExpenseDialog.value = true);
const closeCreateExpenseDialog = () => (showCreateExpenseDialog.value = false);
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-detail {
  @include utilities.flex-column;
}
</style>
