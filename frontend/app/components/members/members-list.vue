<template>
  <div class="members">
    <h2>Members</h2>
    <p v-if="isPending">Loading...</p>
    <p v-else-if="error">{{ error }}</p>
    <div v-else>
      <pre>{{ JSON.stringify(data, null, 2) }}</pre>
    </div>
  </div>
</template>

<script setup lang="ts">
import { getMembers } from '#shared/api-clients/split-the-bill-api/api-client';

const { data, error, status } = useLazyAsyncData('members', getMembers);
const isPending = computed(() => status.value === 'pending');
</script>
