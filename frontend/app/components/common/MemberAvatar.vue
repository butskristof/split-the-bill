<template>
  <Avatar
    :label="label"
    size="large"
    shape="circle"
  />
</template>

<script setup lang="ts">
type Member = {
  id: string;
  name: string;
};
const props = defineProps<{
  member: Member;
}>();

const label = computed(() => props.member.name.charAt(0).toUpperCase());
</script>

<style scoped lang="scss"></style>
