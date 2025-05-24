<template>
  <div class="group-detail-page">
    <AppPageBackButton />
    <AppPageMain>
      <h1>{{ groupId }}</h1>
    </AppPageMain>
  </div>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';

const route = useRoute();
const groupId = route.params.id as string;
</script>
