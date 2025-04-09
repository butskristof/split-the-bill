<template>
  <NuxtLink
    class="group-list-item"
    :to="`/groups/${group.id}`"
  >
    <UAvatar
      size="xl"
      class="icon"
      :alt="group.name"
    />
    <div class="name">
      {{ group.name }}
    </div>
    <UIcon
      name="i-mynaui-arrow-right"
      class="open-arrow"
    />
  </NuxtLink>
</template>

<script setup lang="ts">
import type { components } from '#shared/api-clients/split-the-bill-api/spec';

defineProps<{
  group: components['schemas']['GetGroups.Response.GroupDto'];
}>();
</script>

<style scoped>
@reference '~/assets/css/main.css';

.group-list-item {
  display: flex;
  gap: 1rem;
  align-items: center;

  padding: 1rem;
  border-radius: var(--ui-radius);
  position: relative;

  .name {
    flex-grow: 1;
  }

  .open-arrow {
    @apply size-5;
    opacity: 0;
  }

  &:hover {
    background-color: var(--ui-bg-elevated);

    .icon {
      background-color: var(--ui-bg-accented);
    }

    .open-arrow {
      opacity: 100;
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
