<template>
  <div class="groups-list">
    <ApiError
      v-if="error"
      :error="error"
    />
    <template v-if="status === 'pending'">
      <GroupsListItemSkeleton
        v-for="i in 3"
        :key="i"
      />
    </template>
    <GroupsListItem
      v-for="group in groups"
      :key="group.id"
      :group="group"
    />
  </div>
</template>

<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import GroupsListItem from '~/components/groups/overview/list/GroupsListItem.vue';
import GroupsListItemSkeleton from '~/components/groups/overview/list/GroupsListItemSkeleton.vue';

const { data, status, error } = useLazyBackendApi('/Groups', { key: 'groups' });
const groups = computed(() => data.value?.groups);
</script>

<style scoped lang="scss"></style>
