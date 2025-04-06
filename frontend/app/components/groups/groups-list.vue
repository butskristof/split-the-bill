<template>
  <div class="groups">
    <div v-if="isPending">
      <GroupListItemSkeleton
        v-for="i in 3"
        :key="i"
      />
    </div>
    <ApiError
      v-else-if="isError"
      :error="error"
    />
    <div
      v-else
      class="groups-list"
    >
      <GroupListItem
        v-for="group in data.groups"
        :key="group.id"
        :group="group"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import GroupListItem from '~/components/groups/group-list-item.vue';
import GroupListItemSkeleton from '~/components/groups/group-list-item-skeleton.vue';

const { getGroups } = useSplitTheBillApi();
const { data, isError, isPending, error } = getGroups();
</script>
