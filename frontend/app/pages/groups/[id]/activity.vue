<template>
  <div class="page-group-detail-activity">
    <GroupActivity
      v-if="group"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import GroupActivity from '~/components/groups/activities/GroupActivity.vue';

const { data: group } = useDetailPageGroup();
</script>

<style scoped lang="scss"></style>
