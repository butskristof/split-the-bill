<template>
  <Dialog
    v-model:visible="visible"
    modal
    header="Create new group"
    :style="{ width: '95%', maxWidth: '750px' }"
  >
    <div class="actions">
      <Button
        label="Cancel"
        severity="secondary"
        @click="visible = false"
      />
      <Button
        type="submit"
        label="Save"
        icon="pi pi-save"
      />
    </div>
  </Dialog>
</template>

<script setup lang="ts">
const visible = defineModel<boolean>('visible', { required: true });

// TODO warn when exiting when form is dirty
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.actions {
  @include utilities.flex-row-justify-end;
}
</style>
