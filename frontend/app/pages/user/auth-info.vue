<template>
  <AppCard tag="main">
    <h1>Authentication info</h1>
    <PreformattedText :value="user" />
  </AppCard>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import AppCard from '~/components/app/AppCard.vue';

const { user } = useOidcAuth();
</script>
