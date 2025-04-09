<template>
  <div class="group-detail-activity">
    <h2>Activity</h2>
    <GroupDetailActivityItem
      v-for="activity in activities"
      :key="activity"
    />
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  expenses: string[];
  payments: string[];
}>();

const activities = computed(() => [...props.expenses, ...props.payments]);
</script>

<style scoped>
@reference '~/assets/css/main.css';

h2 {
  @apply text-2xl font-bold mb-4;
}
</style>
