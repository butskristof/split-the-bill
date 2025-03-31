<template>
  <header class="backdrop-blur">
    <UContainer class="header-content">
      <AppTitle />

      <div class="menu-items">
        <UAvatar />
      </div>
    </UContainer>
  </header>
</template>

<style scoped>
header {
  @apply bg-(--ui-bg)/75;
  border-bottom: 1px solid var(--ui-border);
  height: var(--ui-header-height);
  position: sticky;
  top: 0;
  z-index: 50;

  display: flex;
  justify-content: center;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
