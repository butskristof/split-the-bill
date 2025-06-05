<template>
  <main v-if="group">
    <h1>{{ group.name }}</h1>
    <AppCard>
      <GroupDetailMembers :members="group.members!" />
    </AppCard>
    <AppCard>
      <GroupDetailRecentActivity :group="group" />
    </AppCard>
    <DeleteGroup :group="group" />
  </main>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import GroupDetailMembers from '~/components/groups/detail/GroupDetailMembers.vue';
import GroupDetailRecentActivity from '~/components/groups/detail/GroupDetailRecentActivity.vue';
import DeleteGroup from '~/components/groups/edit/DeleteGroup.vue';

const { group } = useDetailPageGroup();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

main {
  @include utilities.flex-column;
}
</style>
