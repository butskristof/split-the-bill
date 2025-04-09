<template>
  <NuxtLink
    to="/"
    class="app-title"
  >
    <UIcon
      name="i-mynaui-envelope"
      class="icon"
    />
    Split the bill
  </NuxtLink>
</template>

<style scoped>
.app-title {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 0.5rem;

  font-size: 1.5rem;
  font-weight: bold;

  .icon {
    margin-top: 2px;
    font-size: 125%;
  }
}
</style>
