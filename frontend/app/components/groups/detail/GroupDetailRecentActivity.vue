<template>
  <div class="recent-activity">
    <div class="header">
      <h2>Recent activity</h2>
      <div class="add">
        <Button
          icon="pi pi-plus"
          label="Add"
          aria-haspopup="true"
          aria-controls="add_activity_menu"
          @click="toggleMenu"
        />
        <Menu
          id="add_activity_menu"
          ref="menu"
          :model="menuItems"
          :popup="true"
        />
      </div>
    </div>
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
    <div class="actions">
      <Button
        icon="pi pi-arrow-right"
        icon-pos="right"
        label="All activity"
        variant="text"
      />
    </div>
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

const menuItems = [
  {
    icon: 'pi pi-tag',
    label: 'Expense',
  },
  {
    icon: 'pi pi-money-bill',
    label: 'Payment',
  },
];
const menu = ref();
const toggleMenu = (event: Event) => menu.value?.toggle(event);
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

.header {
  @include utilities.flex-row-justify-between-align-center;
}

.actions {
  @include utilities.flex-row-justify-end;
}
</style>
