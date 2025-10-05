<template>
  <div class="page-group-detail">
    <GroupDetail :group-id="groupId" />
  </div>
</template>

<script setup lang="ts">
import GroupDetail from '~/components/groups/detail/GroupDetail.vue';

const route = useRoute();
const groupId = computed(() => route.params.id as string);
</script>

<style scoped lang="scss"></style>
