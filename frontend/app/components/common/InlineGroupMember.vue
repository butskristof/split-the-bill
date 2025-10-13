<template>
  <span class="member">
    <MemberAvatar
      :member="member"
      size="small"
    />
    <span>
      {{ memberName }}
    </span>
  </span>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import type { Member } from '#shared/types/member';

const props = withDefaults(
  defineProps<{
    memberId?: string | null;
    member?: Member | null;
  }>(),
  {
    memberId: null,
    member: null,
  },
);

const { data: group } = useDetailPageGroup();

const member = computed<Member | null>(() => {
  if (props.member) return props.member;
  if (props.memberId && group.value?.members) {
    return (group.value.members.find((m) => m.id === props.memberId) as Member) ?? null;
  }
  return null;
});
const memberName = computed<string>(() => member.value?.name ?? 'Unknown group member');
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.member {
  display: inline-flex;
  flex-direction: row;
  align-items: center;
  gap: calc(var(--default-spacing) / 2);
}
</style>
