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
// this component provides a ready-to-use modal
// prefer updating and expanding this component to keep it uniform across the application
// possible future expansions: header slot, click outside, ...
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

// capture updates from the Dialog (e.g. close button in header clicked)
// and propagate as close emit if applicable
const updateVisible = (visible: boolean) => {
  if (!visible) {
    emit('close');
  }
};
</script>

<style scoped lang="scss"></style>
