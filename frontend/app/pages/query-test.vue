<template>
  <PreformattedText :value="data" />
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query';
import PreformattedText from '~/components/common/PreformattedText.vue';

const { $backendApi } = useNuxtApp();
const { data, suspense } = useQuery({
  queryKey: ['groups'],
  queryFn: () => $backendApi('/Groups'),
});

await suspense();
</script>

<style scoped lang="scss"></style>
