<template>
  <div class="recent-activity">
    <h2>Recent activity</h2>
    <div class="activities">
      <div
        v-for="activity in recentActivity"
        :key="activity.id"
        class="activity-item"
      >
        <ExpenseActivityListItem
          v-if="activity.type === 'expense'"
          :expense="activity"
          :members="props.group.members?.map((m) => ({ id: m.id!, name: m.name! })) ?? []"
        />
        <PaymentActivityListItem
          v-else-if="activity.type === 'payment'"
          :payment="activity"
          :members="props.group.members?.map((m) => ({ id: m.id!, name: m.name! })) ?? []"
        />
      </div>
    </div>
    <div class="all-activity">
      <Button
        label="Show all activity"
        icon="pi pi-arrow-right"
        variant="text"
        icon-pos="right"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Group, Expense, Payment } from '#shared/types/api';
import PaymentActivityListItem from '~/components/groups/activities/PaymentActivityListItem.vue';
import ExpenseActivityListItem from '~/components/groups/activities/ExpenseActivityListItem.vue';

type ExpenseActivity = Expense & { type: 'expense' };
type PaymentActivity = Payment & { type: 'payment' };
type Activity = ExpenseActivity | PaymentActivity;

const props = defineProps<{
  group: Group;
}>();

const recentActivity = computed<Activity[]>(() => {
  const activity: Activity[] = [];

  // Cast from loose OpenAPI types to strict DTOs at the boundary
  const expenses = (props.group.expenses ?? []) as Expense[];
  const payments = (props.group.payments ?? []) as Payment[];

  activity.push(...expenses.map((e): ExpenseActivity => ({ ...e, type: 'expense' })));
  activity.push(...payments.map((p): PaymentActivity => ({ ...p, type: 'payment' })));

  activity.sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime());

  return activity.slice(0, 3);
});
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.recent-activity {
  @include utilities.flex-column;
}

.activities {
  @include utilities.flex-column;
}

.all-activity {
  @include utilities.flex-row-justify-end;
}
</style>
