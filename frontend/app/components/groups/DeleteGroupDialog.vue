<template>
  <AppDialog
    header="Delete Group"
    @close="tryClose"
  >
    <div class="content">
      <p>
        Are you sure you want to delete the group <strong>{{ props.group.name }}</strong
        >? This action cannot be undone.
      </p>

      <div class="footer">
        <div class="messages">
          <Message
            v-if="deleteGroupMutation.isSuccess.value"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            Group deleted successfully!
          </Message>
          <Message
            v-if="apiErrorTitle"
            severity="error"
            variant="simple"
          >
            {{ apiErrorTitle }}
          </Message>
        </div>
        <div class="actions">
          <Button
            label="Cancel"
            severity="secondary"
            :disabled="deleteGroupMutation.isPending.value || deleteGroupMutation.isSuccess.value"
            @click="tryClose"
          />
          <Button
            icon="pi pi-trash"
            severity="danger"
            :loading="deleteGroupMutation.isPending.value"
            :disabled="deleteGroupMutation.isPending.value || deleteGroupMutation.isSuccess.value"
            :label="deleteGroupMutation.isPending.value ? 'Deleting...' : 'Delete'"
            @click="onDelete"
          />
        </div>
      </div>
    </div>
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import { useDeleteGroupMutation } from '~/composables/backend-api/useDeleteGroupMutation';
import type { FetchError } from 'ofetch';

type Group = {
  id: string;
  name: string;
};

const props = defineProps<{
  group: Group;
}>();
const emit = defineEmits<{
  close: [];
}>();
const tryClose = () => {
  emit('close');
};

const deleteGroupMutation = useDeleteGroupMutation();
const apiErrorTitle = computed<string | null>(() => {
  if (!deleteGroupMutation.error.value) return null;
  const problemDetails = (deleteGroupMutation.error.value as FetchError)?.data as ProblemDetails;
  return problemDetails?.title ?? 'Something went wrong, please try again later.';
});

const onDelete = async () => {
  deleteGroupMutation.mutate(props.group.id, {
    onSuccess: () => {
      // Show success message briefly before navigating
      setTimeout(() => {
        navigateTo({ name: 'groups' });
      }, 1000);
    },
  });
};
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.content {
  @include utilities.flex-column;

  .footer {
    @include utilities.flex-row-justify-between;
    flex-wrap: wrap;

    .actions {
      @include utilities.flex-row-justify-end;
      margin-left: auto;
    }
  }
}
</style>
