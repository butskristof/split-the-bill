<template>
  <div class="page-group-detail">
    <div class="page-header">
      <h1>{{ groupId }}</h1>
    </div>
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const groupId = route.params.id;
</script>

<style scoped lang="scss"></style>
