<template>
  <Avatar
    :label="label"
    :size="size"
    :class="{ small: size === 'small' }"
    shape="circle"
  />
</template>

<script setup lang="ts">
import type { AvatarProps } from 'primevue/avatar';

type Member = {
  id: string;
  name: string;
};

export type MemberAvatarSize = 'small' | AvatarProps['size'];

const props = withDefaults(
  defineProps<{
    member?: Member | null;
    size?: MemberAvatarSize;
    overflowCount?: number;
  }>(),
  {
    member: null,
    size: 'large',
    overflowCount: undefined,
  },
);

const label = computed(() => {
  if (props.overflowCount !== undefined) {
    return `+${props.overflowCount}`;
  }
  return props.member?.name.charAt(0).toUpperCase() ?? '?';
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
