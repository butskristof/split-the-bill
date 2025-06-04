<template>
  <div class="members">
    <h2>Members</h2>
    <ul>
      <li
        v-for="member in members"
        :key="member.id"
        class="member"
      >
        {{ member.name }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import type { GroupMember } from '~/components/groups/detail/types';

defineProps<{
  members: GroupMember[];
}>();
</script>

<style scoped lang="scss"></style>
