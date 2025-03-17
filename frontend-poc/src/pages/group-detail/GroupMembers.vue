<template>
  <h2>Members</h2>
  <GroupMember
    v-for="member in members"
    :key="member.id"
    :member="member"
  />
</template>

<script setup lang="ts">
import type { GroupDto } from '@/types/groups';
import { computed } from 'vue';
import GroupMember from '@/pages/group-detail/GroupMember.vue';

const props = defineProps<{
  group: GroupDto;
}>();
const members = computed(() => props.group.members);
</script>

<style scoped></style>
