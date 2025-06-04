<template>
  <div class="members">
    <h2>Members</h2>
    <div class="members">
      <GroupDetailMemberListItem
        v-for="member in members"
        :key="member.id"
        :member="member"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { GroupMember } from '~/components/groups/detail/types';
import GroupDetailMemberListItem from '~/components/groups/detail/GroupDetailMemberListItem.vue';

defineProps<{
  members: GroupMember[];
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.members {
  @include utilities.flex-column(false);
}
</style>
