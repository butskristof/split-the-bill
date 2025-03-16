<template>
  <h2>Members</h2>
  <pre>
    {{ JSON.stringify(members, null, 2) }}
  </pre>
</template>

<script setup lang="ts">
import type { GroupDto } from '@/types/groups';
import { computed } from 'vue';

const props = defineProps<{
  group: GroupDto;
}>();
const members = computed(() => props.group.members);
</script>

<style scoped></style>
