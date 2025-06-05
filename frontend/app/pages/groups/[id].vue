<template>
  <div class="group-detail-page">
    <AppPageBackButton :default-route="{ name: 'groups' }" />
    <PageLoadingIndicator v-if="status === 'pending'" />
    <ApiError
      v-if="error"
      :error="error"
    />
    <NuxtPage v-if="group" />
  </div>
</template>

<script setup lang="ts">
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';
import ApiError from '~/components/common/ApiError.vue';
import PageLoadingIndicator from '~/components/common/PageLoadingIndicator.vue';

const route = useRoute();
const groupId = route.params.id as string;
const key = computed(() => `groups/${groupId}`);
const {
  data: group,
  status,
  error,
} = await useLazyBackendApi('/Groups/{id}', { key, path: { id: groupId } });

const { provideGroup } = useDetailPageGroup();
provideGroup(group);
</script>
