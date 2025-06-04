<template>
  <NuxtLink
    class="member"
    to="#"
  >
    <div class="avatar-name">
      <Avatar
        shape="circle"
        :label="label"
      />
      <div class="name">{{ member.name }}</div>
    </div>
    <div class="balance">
      <template v-if="member.totalBalance === 0">
        <span>is settled up</span>
      </template>
      <template v-else>
        <span v-if="member.totalBalance > 0">is owed</span>
        <span v-else-if="member.totalBalance < 0">owes</span>
        &nbsp;
        <strong>{{ formattedTotalBalance }}</strong>
      </template>
    </div>
    <i class="pi pi-arrow-right" />
  </NuxtLink>
</template>

<script setup lang="ts">
import { formatCurrency, getUpperCaseFirstLetter } from '#shared/utils';
import type { GroupMember } from '~/components/groups/detail/types';

const props = defineProps<{
  member: GroupMember;
}>();
const label = computed(() => getUpperCaseFirstLetter(props.member.name));

const formattedTotalBalance = computed(() => formatCurrency(Math.abs(props.member.totalBalance)));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';
.member {
  @include utilities.reset-link;
  @include utilities.flex-row-justify-between-align-center(false);
  margin-inline: calc(var(--default-spacing) * -1);
  padding: var(--default-spacing);
  &:not(:last-child) {
    border-bottom: 1px solid var(--p-surface-200);
  }

  .pi {
    color: var(--p-surface-400);
    margin-left: var(--default-spacing);
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

  .avatar-name {
    @include utilities.flex-row-align-center;
    flex-grow: 1;
  }

  @include utilities.dark-mode {
    border-bottom-color: var(--p-surface-700);
  }
}
</style>
