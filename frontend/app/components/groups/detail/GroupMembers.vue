<template>
  <div class="header">
    <h2>Members</h2>
    <UButton
      label="Add"
      icon="i-mynaui-plus"
    />
  </div>
  <div class="member-list">
    <div
      v-for="member in members"
      :key="member.id"
      class="member-list-item"
    >
      <UAvatar
        :alt="member.name"
        class="avatar"
      />
      <h3 class="name">{{ member.name }}</h3>
      <p
        class="balance"
        :class="{ positive: member.totalBalance > 0, negative: member.totalBalance < 0 }"
      >
        <span v-if="member.totalBalance === 0">settled up</span>
        <span v-else>
          {{ member.totalBalance > 0 ? 'gets back' : 'owes' }}
          {{ formatCurrency(Math.abs(member.totalBalance)) }}
        </span>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { GroupMember } from '#shared/api-clients/split-the-bill-api/types';
import { formatCurrency } from '~~/shared/utilities/format-currency';

defineProps<{
  members: GroupMember[];
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.header {
  @include utilities.flex-row-justify-between;
}

.member-list-item {
  @include utilities.flex-row-align-center;
  padding: var(--default-spacing);
  border-radius: var(--ui-radius);
  @include utilities.list-item-hover;
  @include utilities.list-item-separator;

  .name {
    flex-grow: 1;
  }

  .balance {
    @include utilities.semibold;

    &.positive {
      //color: green;
    }

    &.negative {
      //color: red;
    }
  }

  &:hover {
    .avatar {
      background-color: var(--ui-bg-accented);
    }
  }
}
</style>
