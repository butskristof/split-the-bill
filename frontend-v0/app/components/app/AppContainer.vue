<template>
  <div class="container">
    <slot />
  </div>
</template>

<style scoped lang="scss">
.container {
  max-width: var(--app-container);
  margin-inline: auto;
}
</style>
