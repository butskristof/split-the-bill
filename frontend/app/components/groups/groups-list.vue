<template>
  <div class="groups">
    <p v-if="isPending">Loading...</p>
    <ApiError
      v-if="isError"
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

const { getGroups } = useSplitTheBillApi();
const { data, isError, isPending, error } = getGroups();
</script>
