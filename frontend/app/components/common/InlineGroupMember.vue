<template>
  <span>
    {{ memberName }}
  </span>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';

type Member = {
  id: string;
  name: string;
};

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

<style scoped lang="scss"></style>
