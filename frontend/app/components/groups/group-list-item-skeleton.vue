<template>
  <div class="group-list-item">
    <USkeleton class="icon h-10 w-10 rounded-full" />
    <USkeleton class="name h-5 w-50" />
  </div>
</template>

<style scoped>
@reference '~/assets/css/main.css';

.group-list-item {
  display: flex;
  gap: 1rem;
  align-items: center;

  padding: 1rem;
  border-radius: var(--ui-radius);
  position: relative;

  &:hover {
    background-color: var(--ui-bg-elevated);

    .icon,
    .name {
      background-color: var(--ui-bg-accented);
    }
  }

  &:not(:last-child)::after {
    content: '';
    position: absolute;
    left: 0.5rem;
    right: 0.5rem;
    bottom: 0;
    border-bottom: 1px solid var(--ui-border);
  }
}
</style>
