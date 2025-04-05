<template>
  <div class="groups">
    <h2>Groups</h2>
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
const { getGroups } = useSplitTheBillApi();
const { data, isError, isPending, error } = getGroups();
</script>
