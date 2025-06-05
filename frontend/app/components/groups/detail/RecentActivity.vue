<template>
  <AppCard class="recent-activity">
    <div class="header">
      <h2>Recent activity</h2>
      <CreateActivity />
    </div>
  </AppCard>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import CreateActivity from '~/components/groups/activities/CreateActivity.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.header {
  @include utilities.flex-row-justify-between-align-center;
}
</style>
