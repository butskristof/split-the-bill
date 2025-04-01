<template>
  <div class="groups">
    <h2>Groups</h2>
    <p v-if="isPending">Loading...</p>
    <p v-if="error">{{ error }}</p>
    <ul v-else>
      <li
        v-for="group in groups"
        :key="group.id"
      >
        {{ group.name }} ({{ group.id }})
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
const { data, error, status } = await useSplitTheBillApi('/Groups', { lazy: true });
const isPending = computed(() => status.value === 'pending');
const groups = computed(() => data?.value?.groups);
</script>
