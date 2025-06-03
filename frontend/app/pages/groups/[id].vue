<template>
  <div class="group-detail-page">
    <AppPageBackButton :default-route="{ name: 'groups' }" />
    <AppPageMain>
      <PageLoadingIndicator v-if="status === 'pending'" />
      <ApiError
        v-if="error"
        :error="error"
      />
      <template v-if="group">
        <h1>{{ group.name }}</h1>
        <GroupDetailRecentActivity :group="group" />
        <PreformattedText
          v-if="false"
          :value="group"
        />
      </template>
    </AppPageMain>
  </div>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';
import ApiError from '~/components/common/ApiError.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';
import GroupDetailRecentActivity from '~/components/groups/detail/GroupDetailRecentActivity.vue';
import PageLoadingIndicator from '~/components/common/PageLoadingIndicator.vue';

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
