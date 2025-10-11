<template>
  <Dialog
    visible
    modal
    :draggable="false"
    :header="header"
    :style="
      !isMaximized
        ? {
            width: '100%',
            marginInline: 'var(--default-spacing)',
            maxWidth: '48rem',
          }
        : undefined
    "
    maximizable
    @maximize="isMaximized = true"
    @unmaximize="isMaximized = false"
    @update:visible="onUpdateVisible"
  >
    <slot />
  </Dialog>
</template>

<script setup lang="ts">
defineProps<{
  header?: string;
}>();
const emit = defineEmits<{
  close: [];
}>();

const onUpdateVisible = (newValue: boolean) => {
  // visible false -> close dialog
  if (!newValue) emit('close');
};

const isMaximized = ref(false);
</script>

<style scoped lang="scss"></style>
