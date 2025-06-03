<template>
  <div class="group-detail-page">
    <AppPageBackButton :default-route="{ name: 'groups' }" />
    <PageLoadingIndicator v-if="status === 'pending'" />
    <ApiError
      v-if="error"
      :error="error"
    />
    <template v-if="group">
      <main>
        <h1>{{ group.name }}</h1>
        <AppPageCard>
          <GroupDetailMembers :members="group.members!" />
        </AppPageCard>
        <AppPageCard>
          <GroupDetailRecentActivity :group="group" />
        </AppPageCard>
      </main>
    </template>
  </div>
</template>

<script setup lang="ts">
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';
import ApiError from '~/components/common/ApiError.vue';
import GroupDetailRecentActivity from '~/components/groups/detail/GroupDetailRecentActivity.vue';
import PageLoadingIndicator from '~/components/common/PageLoadingIndicator.vue';
import AppPageCard from '~/components/app/AppPageCard.vue';
import GroupDetailMembers from '~/components/groups/detail/GroupDetailMembers.vue';

const route = useRoute();
const groupId = route.params.id as string;
const key = computed(() => `groups/${groupId}`);
const {
  data: group,
  status,
  error,
} = await useLazyBackendApi('/Groups/{id}', { key, path: { id: groupId } });
provide('group', readonly(group));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

main {
  @include utilities.flex-column;
}
</style>
