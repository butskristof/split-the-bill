<template>
  <div class="group-activity">
    <h2>Activity</h2>
    <div class="activities">
      <template
        v-for="activity in activities"
        :key="activity.id"
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
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Expense, Group, Payment } from '#shared/types/api';
import ExpenseActivityListItem from '~/components/groups/activities/ExpenseActivityListItem.vue';
import PaymentActivityListItem from '~/components/groups/activities/PaymentActivityListItem.vue';

type ExpenseActivity = Expense & { type: 'expense' };
type PaymentActivity = Payment & { type: 'payment' };
type Activity = ExpenseActivity | PaymentActivity;

const props = defineProps<{
  group: Group;
}>();

const activities = computed<Activity[]>(() => {
  const activity: Activity[] = [];

  // Cast from loose OpenAPI types to strict DTOs at the boundary
  const expenses = (props.group.expenses ?? []) as Expense[];
  const payments = (props.group.payments ?? []) as Payment[];

  activity.push(...expenses.map((e): ExpenseActivity => ({ ...e, type: 'expense' })));
  activity.push(...payments.map((p): PaymentActivity => ({ ...p, type: 'payment' })));

  activity.sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime());

  return activity;
});
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-activity {
  @include utilities.flex-column;
}

.activities {
  @include utilities.flex-column;
}
</style>
