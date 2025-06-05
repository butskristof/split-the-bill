<template>
  <Button
    icon="pi pi-plus"
    label="Create group"
    @click="openModal"
  />
  <CreateGroupModal
    v-if="showModal"
    @close="closeModal"
  />
</template>

<script setup lang="ts">
import CreateGroupModal from '~/components/groups/edit/CreateGroupModal.vue';

const emit = defineEmits<{
  created: [];
}>();

const showModal = ref<boolean>(false);
const openModal = (): void => {
  showModal.value = true;
};
const closeModal = (created: boolean): void => {
  showModal.value = false;
  if (created) emit('created');
};
</script>
