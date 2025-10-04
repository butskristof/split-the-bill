<template>
  <Avatar
    v-tooltip="member.name"
    size="large"
    shape="circle"
    :label
  />
</template>

<script setup lang="ts">
import { getUpperCaseFirstLetter } from '#shared/utils';

const props = defineProps<{
  member: {
    id: string;
    name: string;
  };
}>();
const label = computed(() => getUpperCaseFirstLetter(props.member.name));
</script>

<style scoped lang="scss"></style>
