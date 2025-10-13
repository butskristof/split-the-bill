<template>
  <AvatarGroup>
    <MemberAvatar
      v-for="member in members"
      :key="member.id"
      :member="member"
    />
  </AvatarGroup>
</template>

<script setup lang="ts">
import MemberAvatar from '~/components/common/MemberAvatar.vue';

type Member = {
  id: string;
  name: string;
};
defineProps<{
  members: Member[];
}>();

// TODO pass in max items (cut off to +X)
</script>

<style scoped lang="scss"></style>
