<template>
  <div class="group-detail-members">
    <h2>Members</h2>
    <div class="members">
      <div
        v-for="member in members"
        :key="member.id"
        class="member"
      >
        <MemberAvatar
          :member="member"
          :tooltip="false"
        />
        <div class="name">{{ member.name }}</div>
        <div class="balance">
          <div>{{ member.totalBalance < 0 ? 'owes' : 'is owed' }}</div>
          <div
            class="balance-amount"
            :class="{ positive: member.totalBalance >= 0, negative: member.totalBalance < 0 }"
          >
            <strong>{{ formatCurrency(Math.abs(member.totalBalance)) }}</strong>
          </div>
        </div>
      </div>
    </div>
    <div class="manage-members">
      <NuxtLink
        class="link"
        :to="{ name: 'groups-id-members', params: { id: props.group.id } }"
      >
        <Button
          label="Manage members"
          icon="pi pi-arrow-right"
          variant="text"
          icon-pos="right"
        />
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import type { Group } from '#shared/types/api';
import { formatCurrency } from '#shared/utils';
import type { Member as MinimalMember } from '#shared/types/member';

type Member = MinimalMember & {
  totalBalance: number;
};
const props = defineProps<{
  group: Group;
}>();
const members = computed(() => (props.group.members as Member[]) ?? []);
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.group-detail-members {
  @include utilities.flex-column;
}

.members {
  padding-inline: var(--default-spacing);
  @include utilities.flex-row;
  gap: calc(var(--default-spacing) * 2);
  overflow-x: auto;
}

.member {
  padding-inline: calc(var(--default-spacing) / 2);
  @include utilities.flex-column(false);
  gap: calc(var(--default-spacing) / 2);
  align-items: center;
}

.balance {
  text-align: center;

  .balance-amount {
    font-weight: var(--font-weight-semibold);
  }

  .positive {
    color: var(--p-green-600);
  }

  .negative {
    color: var(--p-red-600);
  }
}

.manage-members {
  @include utilities.flex-row-justify-end;
}
</style>
