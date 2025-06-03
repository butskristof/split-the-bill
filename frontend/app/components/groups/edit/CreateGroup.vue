<template>
  <AppModal
    header="Create new group"
    @close="$emit('close')"
  >
    <div>hey</div>
  </AppModal>
</template>

<script setup lang="ts">
import AppModal from '~/components/common/AppModal.vue';

defineEmits<{
  close: [];
}>();
</script>
