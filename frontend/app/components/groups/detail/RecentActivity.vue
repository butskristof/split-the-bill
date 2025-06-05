<template>
  <AppCard class="recent-activity">
    <div class="header">
      <h2>Recent activity</h2>
      <CreateActivity />
    </div>
    <div class="actions">
      <NuxtLink :to="{ name: 'groups-id-activity', params: { id: group.id } }">
        <Button
          icon="pi pi-arrow-right"
          icon-pos="right"
          label="All activity"
          variant="text"
        />
      </NuxtLink>
    </div>
  </AppCard>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import CreateActivity from '~/components/groups/activities/CreateActivity.vue';
import type { Group } from '~/types';

defineProps<{
  group: Group;
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.recent-activity {
  @include utilities.flex-column;
}

.header {
  @include utilities.flex-row-justify-between-align-center;
}

.actions {
  @include utilities.flex-row-justify-end;
}
</style>
