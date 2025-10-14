<template>
  <div class="page-group-detail-activity">
    <AppBackButton
      label="Back to group overview"
      :to="{ name: 'groups-id', params: { id: groupId } }"
    />
    <GroupActivity
      v-if="group"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import GroupActivity from '~/components/groups/activities/GroupActivity.vue';
import AppBackButton from '~/components/app/AppBackButton.vue';

const { group, groupId } = useDetailPageGroup();
</script>

<style scoped lang="scss"></style>
