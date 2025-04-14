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
      class="open-arrow size-5"
    />
  </NuxtLink>
</template>

<script setup lang="ts">
import type { components } from '#shared/api-clients/split-the-bill-api/spec';

defineProps<{
  group: components['schemas']['GetGroups.Response.GroupDto'];
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.group-list-item {
  @include utilities.flex-row-align-center;
  padding: var(--default-spacing);
  border-radius: var(--ui-radius);
  @include utilities.list-item-hover;
  @include utilities.list-item-separator;

  .name {
    flex-grow: 1;
  }

  .open-arrow {
    opacity: 0;
  }

  &:hover {
    .icon {
      background-color: var(--ui-bg-accented);
    }

    .open-arrow {
      opacity: 100;
    }
  }
}
</style>
