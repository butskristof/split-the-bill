<template>
  <div class="group-name">
    <UAvatar
      size="xl"
      class="icon"
      :alt="name"
    />
    <h1 class="name">{{ name }}</h1>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  name: string;
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.group-name {
  @include utilities.flex-row-align-center;
}
</style>
