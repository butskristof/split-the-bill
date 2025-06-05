<template>
  <AppModal
    header="Delete group"
    @close="$emit('close')"
  >
    <div class="delete-group">
      <div class="message">
        Are you sure you want to delete the group <strong>{{ group.name }}</strong
        >? <br />
        This action cannot be undone.
      </div>
      <Message
        v-if="isError"
        severity="error"
        icon="pi pi-exclamation-circle"
        >Failed to delete the group, please try again later.</Message
      >
      <div class="actions">
        <Button
          icon="pi pi-times"
          label="Cancel"
          severity="secondary"
          :disabled="isDeleting"
          @click="$emit('close')"
        />
        <Button
          icon="pi pi-trash"
          severity="danger"
          label="Delete"
          :loading="isDeleting"
          :disabled="isDeleting"
          @click="onConfirm"
        />
      </div>
    </div>
  </AppModal>
</template>

<script setup lang="ts">
import AppModal from '~/components/common/AppModal.vue';
import type { Group } from '~/types';

const { $backendApi } = useNuxtApp();
const toast = useToast();

const props = defineProps<{
  group: Group;
}>();
const emit = defineEmits<{
  close: [];
}>();

const isDeleting = ref(false);
const isError = ref(false);

const onConfirm = async () => {
  isError.value = false;
  isDeleting.value = true;

  try {
    await $backendApi('/Groups/{id}', {
      method: 'DELETE',
      path: { id: props.group.id! },
    });

    toast.add({
      severity: 'success',
      summary: 'Group deleted successfully',
    });
    emit('close');
    await navigateTo({ name: 'groups' });
  } catch (error) {
    isError.value = true;
    console.error(error);
  } finally {
    isDeleting.value = false;
  }
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.delete-group {
  @include utilities.flex-column;
}

.actions {
  @include utilities.flex-row-justify-between-align-center;
  margin-top: var(--default-spacing);
}
</style>
