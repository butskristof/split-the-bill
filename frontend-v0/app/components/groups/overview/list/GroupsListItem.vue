<template>
  <NuxtLink
    class="list-item"
    :to="{ name: 'groups-id', params: { id: group.id } }"
  >
    <div class="left">
      <Avatar
        shape="circle"
        :label="label"
      />
      <div class="name">{{ group.name }}</div>
    </div>
    <div class="right">
      <i class="pi pi-arrow-right" />
    </div>
  </NuxtLink>
</template>

<script setup lang="ts">
import { getUpperCaseFirstLetter } from '#shared/utils';
import type { GetGroupsResponse } from '~/components/groups/overview/types';

type Group = GetGroupsResponse['groups'][number];

const props = defineProps<{
  group: Group;
}>();
const label = computed(() => getUpperCaseFirstLetter(props.group.name));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.list-item {
  @include utilities.reset-link;
  @include utilities.flex-row-justify-between-align-center;
  margin-inline: calc(var(--default-spacing) * -1);
  padding: var(--default-spacing);
  &:not(:last-child) {
    border-bottom: 1px solid var(--p-surface-200);
  }

  .pi {
    color: var(--p-surface-400);
  }

  &:hover {
    transition: background 0.2s;

    background-color: var(--p-surface-100);
    @include utilities.dark-mode {
      background-color: var(--p-surface-800);
    }

    .pi {
      transition: all 0.1s;
      translate: -0.25rem;
      color: var(--p-surface-600);

      @include utilities.dark-mode {
        color: var(--p-surface-0);
      }
    }
  }

  @include utilities.dark-mode {
    border-bottom-color: var(--p-surface-700);
  }

  .left {
    @include utilities.flex-row-justify-between-align-center;
  }
}
</style>
