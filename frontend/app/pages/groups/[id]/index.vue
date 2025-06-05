<template>
  <main v-if="group">
    <h1>{{ group.name }}</h1>
    <AppPageCard>
      <GroupDetailMembers :members="group.members!" />
    </AppPageCard>
    <AppPageCard>
      <GroupDetailRecentActivity :group="group" />
    </AppPageCard>
    <div class="delete">
      <Button
        icon="pi pi-trash"
        label="Delete group"
        severity="danger"
        variant="text"
        @click="showDelete = true"
      />
      <DeleteGroup
        v-if="showDelete"
        :group="group"
        @close="showDelete = false"
      />
    </div>
  </main>
</template>

<script setup lang="ts">
import AppPageCard from '~/components/app/AppPageCard.vue';
import GroupDetailMembers from '~/components/groups/detail/GroupDetailMembers.vue';
import GroupDetailRecentActivity from '~/components/groups/detail/GroupDetailRecentActivity.vue';
import DeleteGroup from '~/components/groups/edit/DeleteGroup.vue';

const showDelete = ref(false);

const { group } = useDetailPageGroup();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

main {
  @include utilities.flex-column;
}

.delete {
  @include utilities.flex-row-justify-end;
}
</style>
