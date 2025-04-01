<template>
  <div class="group-detail-page">
    <UButton
      icon="i-mynaui-arrow-left"
      to="/groups"
      >Back to overview</UButton
    >
    <h1>Group detail</h1>
    <h2>{{ id }}</h2>

    <p v-if="isPending">Loading...</p>
    <p v-if="error">{{ error }}</p>
    <pre v-else>{{ JSON.stringify(data, null, 2) }}</pre>
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const id = route.params.id![0]!;

const { data, error, status } = useSplitTheBillApi('/Groups/{id}', {
  path: { id },
  lazy: true,
});
const isPending = computed(() => status.value === 'pending');
</script>
