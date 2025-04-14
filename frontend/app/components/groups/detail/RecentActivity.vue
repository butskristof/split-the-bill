<template>
  <div class="header">
    <h2>Recent activity</h2>
    <HeaderDropdownMenu
      :items="headerDropdownItems"
      label="Add"
      icon="i-mynaui-plus"
    />
  </div>
  <div class="activity-list" />
  <RecentActivityListItem
    v-for="activity in recentActivities"
    :key="activity.id"
    :activity="activity"
  />
  <PreformattedText :value="recentActivities" />
  <div class="footer">
    <UButton
      variant="ghost"
      trailing-icon="i-mynaui-arrow-right"
      label="Show all activity"
    />
  </div>
</template>

<script setup lang="ts">
import HeaderDropdownMenu from '~/components/common/HeaderDropdownMenu.vue';
import type { components } from '#shared/api-clients/split-the-bill-api/spec';
import PreformattedText from '~/components/common/PreformattedText.vue';
import RecentActivityListItem from '~/components/groups/detail/RecentActivityListItem.vue';

const props = defineProps<{
  group: components['schemas']['GroupDto'];
}>();

//#region header
const headerDropdownItems = [
  {
    label: 'Add expense',
    icon: 'i-material-symbols-payments-outline',
  },
  {
    label: 'Add payment',
    icon: 'i-hugeicons-save-money-euro',
  },
];
//#endregion

const recentActivities = computed(() => {
  const activities = [...(props.group.expenses ?? []), ...(props.group.payments ?? [])];
  return activities;
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.header {
  @include utilities.flex-row-justify-between;
}

.footer {
  @include utilities.flex;
  justify-content: flex-end;
}
</style>
