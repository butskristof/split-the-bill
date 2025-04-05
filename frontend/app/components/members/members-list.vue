<template>
  <div class="members">
    <h2>Members</h2>
    <p v-if="isPending">Loading...</p>
    <div v-if="isError">
      <h3>Error</h3>
      <pre>{{ JSON.stringify(error, null, 2) }}</pre>
    </div>
    <div v-else>
      <h3>Data</h3>
      <pre>{{ JSON.stringify(data, null, 2) }}</pre>
    </div>
  </div>
</template>

<script setup lang="ts">
const { getMembers } = useSplitTheBillApi();
const { data, isError, isPending, error } = getMembers();
</script>
