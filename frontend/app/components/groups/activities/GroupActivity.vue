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
        />
        <PaymentActivityListItem
          v-else-if="activity.type === 'payment'"
          :payment="activity"
        />
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Group } from '#shared/types/api';
import ExpenseActivityListItem from '~/components/groups/activities/ExpenseActivityListItem.vue';
import PaymentActivityListItem from '~/components/groups/activities/PaymentActivityListItem.vue';
import { getActivities } from '~/composables/backend-api/useDetailPageGroup';

const props = defineProps<{
  group: Group;
}>();
const activities = computed(() => getActivities(props.group));
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
