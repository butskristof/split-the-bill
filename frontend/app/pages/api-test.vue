<template>
  <AppPageMain>
    <h1>API test</h1>
    <Button
      label="refresh"
      @click="() => refresh()"
    />
    <PageLoadingIndicator
      v-if="isPending"
      entity="group"
    />
    <PreformattedText
      v-if="data"
      :value="data"
    />
    <ApiError
      v-if="error"
      :error="error"
    />
  </AppPageMain>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';
import ApiError from '~/components/common/ApiError.vue';
import PageLoadingIndicator from '~/components/common/PageLoadingIndicator.vue';

const { data, error, refresh, status } = await useLazyBackendApi('/Members', {
  key: 'members',
});
const isPending = computed(() => status.value === 'pending');
</script>
