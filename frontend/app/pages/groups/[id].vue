<template>
  <h1>{{ id }}</h1>
</template>
<script setup lang="ts">
const route = useRoute();
const id = route.params.id;
</script>
