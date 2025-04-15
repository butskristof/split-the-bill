<template>
  <div class="group-detail">
    <BackButton to="/groups">Back to groups overview</BackButton>

    <LoadingIndicator
      v-if="isPending"
      entity="group"
    />

    <ApiError
      v-else-if="isError"
      :error="error"
    />

    <template v-if="group">
      <GroupName :name="group.name" />
      <RecentActivity />
      <GroupMembers :members="group.members" />
    </template>
  </div>
</template>

<script setup lang="ts">
import BackButton from '~/components/common/BackButton.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import ApiError from '~/components/common/ApiError.vue';
import GroupName from '~/components/groups/detail/GroupName.vue';
import GroupMembers from '~/components/groups/detail/GroupMembers.vue';
import { useProvideGroup } from '~/composables/group';
import RecentActivity from '~/components/groups/detail/RecentActivity.vue';

const { group, isPending, isError, error } = await useGroup();
useProvideGroup(group);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.group-detail {
  @include utilities.flex-column;
}
</style>
