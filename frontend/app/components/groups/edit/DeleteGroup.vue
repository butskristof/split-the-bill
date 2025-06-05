<template>
  <div class="delete-group">
    <Button
      icon="pi pi-trash"
      label="Delete group"
      severity="danger"
      variant="text"
      @click="openModal"
    />
    <DeleteGroupModal
      v-if="showModal"
      :group="group"
      @close="closeModal"
    />
  </div>
</template>

<script setup lang="ts">
import type { Group } from '~/types';
import DeleteGroupModal from '~/components/groups/edit/DeleteGroupModal.vue';

defineProps<{
  group: Group;
}>();

const showModal = ref<boolean>(false);
const openModal = (): void => {
  showModal.value = true;
};
const closeModal = (): void => {
  showModal.value = false;
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.delete-group {
  @include utilities.flex-row-justify-end;
}
</style>
