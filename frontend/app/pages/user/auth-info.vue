<template>
  <AppPageMain>
    <h1>Authentication info</h1>
    <PreformattedText :value="user" />
  </AppPageMain>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import AppPageMain from '~/components/app/AppPageMain.vue';

const { user } = useOidcAuth();
</script>
