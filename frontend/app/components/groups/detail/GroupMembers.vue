<template>
  <div class="members">
    <div class="header">
      <h2>Members</h2>
      <div class="actions">
        <UButton
          icon="i-mynaui-plus"
          label="Add"
        />
      </div>
    </div>
    <GroupMember
      v-for="member in members"
      :key="member.id"
      :member="member"
    />
  </div>
</template>

<script setup lang="ts">
import type { GetGroupResponse } from '#shared/api-clients/split-the-bill-api/types';
import GroupMember from '~/components/groups/detail/GroupMember.vue';

defineProps<{
  members: GetGroupResponse['members'];
}>();
</script>

<style scoped>
@reference '~/assets/css/main.css';

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;

  h2 {
    @apply text-xl font-bold;
  }
}
</style>
