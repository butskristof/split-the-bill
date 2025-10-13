<template>
  <Avatar
    v-tippy="tooltip ? memberName : null"
    :label="label"
    :size="size"
    :class="{ small: size === 'small' }"
    shape="circle"
  />
</template>

<script setup lang="ts">
import type { AvatarProps } from 'primevue/avatar';
import type { Member } from '#shared/types/member';

export type MemberAvatarSize = 'small' | AvatarProps['size'];

const props = withDefaults(
  defineProps<{
    member?: Member | null;
    size?: MemberAvatarSize;
    overflowCount?: number;
    tooltip?: boolean;
  }>(),
  {
    member: null,
    size: 'large',
    overflowCount: undefined,
    tooltip: true,
  },
);

const memberName = computed<string | null>(() => props.member?.name ?? null);
const label = computed(() => {
  if (props.overflowCount !== undefined) {
    return `+${props.overflowCount}`;
  }
  return memberName.value?.charAt(0).toUpperCase() ?? '?';
});
</script>

<style scoped lang="scss">
.small {
  --p-avatar-width: 1.25rem;
  --p-avatar-height: 1.25rem;
  --p-avatar-font-size: 0.625rem;
  vertical-align: middle;
}
</style>
