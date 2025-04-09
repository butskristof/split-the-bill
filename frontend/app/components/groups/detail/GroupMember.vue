<template>
  <div class="member">
    <div class="header">
      <UAvatar class="avatar" />
      <h3 class="name">{{ member.name }}</h3>
      <UIcon
        name="i-mynaui-arrow-right"
        class="open-close-icon"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { components } from '#shared/api-clients/split-the-bill-api/spec';

defineProps<{
  member: components['schemas']['GroupDto.MemberDto'];
}>();
</script>

<style scoped>
@reference '~/assets/css/main.css';

.header {
  display: flex;
  gap: 1rem;
  align-items: center;

  border-radius: var(--ui-radius);
  position: relative;

  .name {
    flex-grow: 1;
  }

  .open-close-icon {
    @apply size-5;
  }
}

.member {
  position: relative;
  margin-bottom: 1rem;
  padding-bottom: 1rem;

  &:not(:last-child)::after {
    content: '';
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
    border-bottom: 1px solid var(--ui-border);
  }
}
</style>
