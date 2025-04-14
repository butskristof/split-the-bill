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

    <template v-else>
      <GroupName :name="group.name" />
      <RecentActivity :group="group" />
      <GroupMembers :members="group.members" />
      <PreformattedText :value="group" />
    </template>
  </div>
</template>

<script setup lang="ts">
import BackButton from '~/components/common/BackButton.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import ApiError from '~/components/common/ApiError.vue';
import GroupName from '~/components/groups/detail/GroupName.vue';
import RecentActivity from '~/components/groups/detail/RecentActivity.vue';
import GroupMembers from '~/components/groups/detail/GroupMembers.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';

const route = useRoute();
const id = route.params.id as string;

const { getGroup } = useSplitTheBillApi();
const { data: group, isError, isPending, error } = getGroup(id);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.group-detail {
  @include utilities.flex-column;
}
</style>
