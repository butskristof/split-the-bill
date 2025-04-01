<template>
  <div class="members">
    <h2>Members</h2>
    <p v-if="isPending">Loading...</p>
    <p v-if="error">{{ error }}</p>
    <ul v-else>
      <MemberListItem
        v-for="member in members"
        :key="member.id"
        :member="member"
      />
    </ul>
  </div>
</template>

<script setup lang="ts">
import MemberListItem from '~/components/members/member-list-item.vue';

const { data, error, status } = await useSplitTheBillApi('/Members', { lazy: true });
const isPending = computed(() => status.value === 'pending');
const members = computed(() => data?.value?.members);
</script>
