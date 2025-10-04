<template>
  <div class="page-auth-info">
    <h1>Authentication info</h1>
    <PreformattedText :value="user" />
  </div>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';

const { user } = useOidcAuth();
</script>

<style scoped lang="scss"></style>
