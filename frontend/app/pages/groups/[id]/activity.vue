<template>
  <main v-if="group">
    <h1>activity</h1>
  </main>
</template>

<script setup lang="ts">
const { group } = useDetailPageGroup();
</script>

<style scoped lang="scss"></style>
