<template>
  <div class="recent-activity">
    <h2>Recent activity</h2>
    <div class="activities">
      <template
        v-for="activity in recentActivity"
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
    <div class="all-activity">
      <NuxtLink
        class="link"
        :to="{ name: 'groups-id-activity', params: { id: props.group.id } }"
      >
        <Button
          label="Show all activity"
          icon="pi pi-arrow-right"
          variant="text"
          icon-pos="right"
        />
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Group } from '#shared/types/api';
import PaymentActivityListItem from '~/components/groups/activities/PaymentActivityListItem.vue';
import ExpenseActivityListItem from '~/components/groups/activities/ExpenseActivityListItem.vue';
import { getActivities } from '~/composables/backend-api/useDetailPageGroup';

const props = defineProps<{
  group: Group;
}>();
const activities = computed(() => getActivities(props.group));
const recentActivity = computed(() => activities.value.slice(0, 3));
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
