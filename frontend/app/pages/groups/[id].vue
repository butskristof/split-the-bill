<template>
  <div class="page-group-detail">
    <GroupDetail
      v-if="group"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import GroupDetail from '~/components/groups/detail/GroupDetail.vue';
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';

const { data: group } = useDetailPageGroup();
</script>

<style scoped lang="scss"></style>
