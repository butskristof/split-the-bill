<template>
  <div class="group-detail-page">
    <PageLoadingIndicator v-if="isPending" />
    <ApiError
      v-if="isError"
      :error="error"
    />
    <NuxtPage v-if="isSuccess" />
  </div>
</template>

<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import PageLoadingIndicator from '~/components/common/PageLoadingIndicator.vue';
import type { Group, Query } from '~/types';

const route = useRoute();
const groupId: string = route.params.id as string;
const key = computed(() => `groups/${groupId}`);
const {
  data: group,
  status,
  error,
}: Query<Group> = await useLazyBackendApi('/Groups/{id}', { key, path: { id: groupId } });
const isPending = computed<boolean>(() => status.value === 'pending');
const isError = computed<boolean>(() => status.value === 'error');
const isSuccess = computed<boolean>(() => status.value === 'success');

const { provideGroup } = useDetailPageGroup();
provideGroup(group);
</script>
