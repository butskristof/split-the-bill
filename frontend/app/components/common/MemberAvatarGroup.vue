<template>
  <AvatarGroup>
    <MemberAvatar
      v-for="member in visibleMembers"
      :key="member.id"
      :member="member"
      :size="size"
    />
    <MemberAvatar
      v-if="overflowCount > 0"
      :overflow-count="overflowCount"
      :size="size"
    />
  </AvatarGroup>
</template>

<script setup lang="ts">
import MemberAvatar, { type MemberAvatarSize } from '~/components/common/MemberAvatar.vue';
import type { Member } from '#shared/types/member';

const props = withDefaults(
  defineProps<{
    members: (Member | null)[];
    maxItems?: number;
    size?: MemberAvatarSize;
  }>(),
  {
    maxItems: 5,
    size: 'large',
  },
);

const hasOverflow = computed(() => props.members.length > props.maxItems);

const visibleMembers = computed(() => {
  const limit = hasOverflow.value ? props.maxItems - 1 : props.maxItems;
  return props.members.slice(0, limit);
});

const overflowCount = computed(() => {
  if (!hasOverflow.value) return 0;
  return props.members.length - (props.maxItems - 1);
});
</script>

<style scoped lang="scss"></style>
