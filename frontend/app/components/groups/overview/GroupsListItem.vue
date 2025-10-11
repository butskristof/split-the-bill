<template>
  <NuxtLink
    :to="{ name: 'groups-id', params: { id: group.id } }"
    class="groups-list-item-link"
  >
    <Card class="groups-list-item">
      <template #title>
        <div class="group-name">{{ group.name }}</div>
        <div class="member-count"><i class="pi pi-users" /> {{ group.memberCount }}</div>
      </template>
      <template #footer>
        <div class="actions">
          <i class="pi pi-arrow-right" />
        </div>
      </template>
    </Card>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { components } from '#open-fetch-schemas/backend-api';
type Group = components['schemas']['GetGroups.Response.GroupDto'];

defineProps<{
  group: Group;
}>();
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list-item-link {
  @include utilities.reset-link;

  // Add smooth transition for hover effects
  .groups-list-item {
    transition: all 0.2s ease-in-out;
    ::v-deep(.p-card-title) {
      @include utilities.flex-row-justify-between-align-center;

      .member-count {
        font-weight: initial;
        font-size: initial;
      }
    }
    ::v-deep(.p-card-footer) {
      @include utilities.flex-row-justify-end;
    }
  }

  // Hover state using PrimeVue theme variables
  &:hover .groups-list-item {
    background: var(--p-content-hover-background);
    border-color: var(--p-content-hover-border-color);
  }

  // Focus state for accessibility
  &:focus {
    outline: none;

    .groups-list-item {
      outline: 2px solid var(--p-focus-ring-color);
      outline-offset: 2px;
    }
  }

  // Active state
  &:active .groups-list-item {
    transform: scale(0.99);
  }
}
</style>
