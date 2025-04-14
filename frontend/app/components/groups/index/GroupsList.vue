<template>
  <div class="groups-list">
    <template v-if="isPending">
      <ListItemSkeleton
        v-for="i in 3"
        :key="i"
      />
    </template>

    <ApiError
      v-else-if="isError"
      :error="error"
    />

    <template v-else>
      <ListItem
        v-for="group in data.groups"
        :key="group.id"
        :group="group"
      />
    </template>
  </div>
</template>

<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import ListItem from './ListItem.vue';
import ListItemSkeleton from './ListItemSkeleton.vue';

const { getGroups } = useSplitTheBillApi();
const { data, isError, isPending, error } = getGroups();
</script>
