<template>
  <AppPageMain>
    <h1>API test</h1>
    <Button
      label="refresh"
      @click="refresh"
    />
    <PreformattedText :value="data" />
  </AppPageMain>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';
const { data, refresh } = await useLazyBackendApi('/Members', {
  key: 'members',
});
</script>
