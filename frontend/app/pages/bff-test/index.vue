<template>
  <AppPageMain>
    <h1>BFF test</h1>
    <div class="composable">
      <PreformattedText :value="composable" />
    </div>
    <div class="session">
      <PreformattedText :value="error" />
      <PreformattedText :value="session" />
    </div>
  </AppPageMain>
</template>

<script setup lang="ts">
import AppPageMain from '~/components/app/AppPageMain.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';

const { user: composable } = useOidcAuth();
const { data: session, error } = await useFetch('/api/backend/members', {
  headers: {
    'x-csrf': '1',
  },
});
</script>
