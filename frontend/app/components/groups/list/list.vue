<template>
  <div class="groups">
    <div v-if="isPending">
      <ListItemSkeleton
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
      <ListItem
        v-for="group in data.groups"
        :key="group.id"
        :group="group"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import ListItem from '~/components/groups/list/list-item.vue';
import ListItemSkeleton from '~/components/groups/list/list-item-skeleton.vue';

const { getGroups } = useSplitTheBillApi();
const { data, isError, isPending, error } = getGroups();
</script>
