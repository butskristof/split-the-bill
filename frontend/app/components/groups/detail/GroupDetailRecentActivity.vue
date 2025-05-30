<template>
  <div class="recent-activity">
    <h2>Recent activity</h2>
    <Timeline
      :value="activities"
      class="timeline"
    >
      <template #content="slotProps">
        <RecentActivityExpense
          v-if="slotProps.item.type === 'Expense'"
          :expense="slotProps.item"
        />
        <RecentActivityPayment
          v-else-if="slotProps.item.type === 'Payment'"
          :payment="slotProps.item"
        />
      </template>
    </Timeline>
  </div>
</template>

<script setup lang="ts">
import type { Group, Activity } from './types';
import RecentActivityExpense from '~/components/groups/detail/RecentActivityExpense.vue';
import RecentActivityPayment from '~/components/groups/detail/RecentActivityPayment.vue';

const props = defineProps<{
  group: Group;
}>();

const activities = computed<Activity[]>(() => {
  const all = [
    ...(props.group.expenses?.map((e) => ({ ...e, type: 'Expense' })) ?? []),
    ...(props.group.payments?.map((p) => ({ ...p, type: 'Payment' })) ?? []),
  ];
  // sort by descending timestamp and take three most recent ones
  return all
    .sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime())
    .slice(0, 3);
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.recent-activity {
  @include utilities.flex-column;

  .timeline {
    ::v-deep(.p-timeline-event-opposite) {
      display: none;
    }

    ::v-deep(.p-timeline-event) {
      min-height: 0;
    }

    ::v-deep(.p-timeline-event-content) {
      margin-top: -0.1rem;
      padding-bottom: var(--default-spacing);
    }
  }
}
</style>
