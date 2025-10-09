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
      <CreateExpenseDialog5
        :group-id="group.id!"
        :members="group.members!.map((m) => ({ id: m.id!, name: m.name! }))"
      />
      <CreateExpenseDialog
        v-if="showCreateExpenseDialog && false"
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
import CreateExpenseDialog5 from '~/components/groups/detail/CreateExpenseDialog5.vue';

const props = defineProps<{
  groupId: string;
}>();

const { data: group, refresh } = await useLazyBackendApi('/Groups/{id}', {
  path: {
    id: props.groupId,
  },
});

//#region create expense dialog
const showCreateExpenseDialog = ref(false);
const openCreateExpenseDialog = () => (showCreateExpenseDialog.value = true);
const closeCreateExpenseDialog = (created: boolean) => {
  showCreateExpenseDialog.value = false;
  if (created) refresh();
};
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-detail {
  @include utilities.flex-column;
}
</style>
