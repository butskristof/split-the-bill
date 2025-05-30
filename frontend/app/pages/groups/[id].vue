<template>
  <div class="group-detail-page">
    <AppPageBackButton :default-route="{ name: 'groups' }" />
    <AppPageMain>
      <LoadingIndicator v-if="status === 'pending'" />
      <ApiError
        v-if="error"
        :error="error"
      />
      <template v-if="group">
        <h1>{{ group.name }}</h1>
        <PreformattedText :value="group" />
      </template>
    </AppPageMain>
  </div>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';
import ApiError from '~/components/common/ApiError.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';

const route = useRoute();
const groupId = route.params.id as string;
const key = computed(() => `groups/${groupId}`);
const {
  data: group,
  status,
  error,
} = await useLazyBackendApi('/Groups/{id}', { key, path: { id: groupId } });
</script>
