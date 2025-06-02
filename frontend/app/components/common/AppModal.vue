<template>
  <Dialog
    visible
    modal
    :draggable="false"
    :style="dialogStyle"
    :header="header"
    @update:visible="updateVisible"
  >
    <slot />
  </Dialog>
</template>

<script setup lang="ts">
defineProps<{
  header: string;
}>();
const emit = defineEmits<{
  close: [];
}>();

const appBreakpoints = useAppBreakpoints();
const maxWidth = `${appBreakpoints.values.md}px`;
const dialogStyle = {
  width: '100%',
  'max-width': maxWidth,
  margin: 'var(--default-spacing)',
};

const updateVisible = (visible: boolean) => {
  if (!visible) {
    emit('close');
  }
};
</script>

<style scoped lang="scss"></style>
