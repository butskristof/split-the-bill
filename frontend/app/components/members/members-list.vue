<template>
  <div class="members">
    <h2>Members</h2>
    <p v-if="isFetching">Loading...</p>
    <p v-else-if="isError">{{ error }}</p>
    <div v-if="data">
      <pre>{{ JSON.stringify(data, null, 2) }}</pre>
    </div>
  </div>
</template>

<script setup lang="ts">
import { getMembers } from '#shared/api-clients/split-the-bill-api/api-client';
import { useQuery } from '@tanstack/vue-query';

const { data, isFetching, isError, error, suspense } = useQuery({
  queryKey: ['members'],
  queryFn: getMembers,
});
await suspense();
</script>
