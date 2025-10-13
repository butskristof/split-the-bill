<template>
  <div class="group-detail-members">
    <h2>Members</h2>
    <div class="members">
      <div
        v-for="member in members"
        :key="member.id"
        class="member"
      >
        <MemberAvatar :member="member" />
        <div class="name">{{ member.name }}</div>
        <div class="balance">{{ member.totalBalance }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import type { Group } from '#shared/types/api';

const props = defineProps<{
  group: Group;
}>();
const members = computed(() => props.group.members ?? []);
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-detail-members {
  @include utilities.flex-column;
}

.members {
  @include utilities.flex-row;
  gap: calc(var(--default-spacing) * 2);
  flex-wrap: wrap;
}

.member {
  @include utilities.flex-column(false);
  align-items: center;
}
</style>
