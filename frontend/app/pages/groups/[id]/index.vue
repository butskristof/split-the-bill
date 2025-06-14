<template>
  <div class="group-detail-page">
    <AppPageBackButton :to="{ name: 'groups' }" />
    <main v-if="group">
      <h1>{{ group.name }}</h1>
      <AppCard>
        <GroupDetailMembers :members="group.members!" />
      </AppCard>
      <RecentActivity :group="group" />
      <DeleteGroup :group="group" />
    </main>
  </div>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import GroupDetailMembers from '~/components/groups/detail/GroupDetailMembers.vue';
import DeleteGroup from '~/components/groups/edit/DeleteGroup.vue';
import RecentActivity from '~/components/groups/detail/RecentActivity.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';

const { group } = useDetailPageGroup();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

main {
  @include utilities.flex-column;
}
</style>
