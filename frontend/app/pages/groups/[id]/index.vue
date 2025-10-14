<template>
  <div class="page-group-detail">
    <AppBackButton
      label="Back to group overview"
      :to="{ name: 'groups' }"
    />
    <GroupDetail
      v-if="group"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import GroupDetail from '~/components/groups/detail/GroupDetail.vue';
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import AppBackButton from '~/components/app/AppBackButton.vue';

const { data: group } = useDetailPageGroup();
</script>

<style scoped lang="scss"></style>
