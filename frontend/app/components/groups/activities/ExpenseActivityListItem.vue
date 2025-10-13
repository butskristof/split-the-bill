<template>
  <NuxtLink
    :to="{
      name: 'groups-id-expenses-expenseId',
      params: {
        id: groupId,
        expenseId: expense.id,
      },
    }"
    class="expense-card-link"
  >
    <Card class="expense">
      <template #content>
        <div class="members-avatars">
          <MemberAvatar :member="paidBy" />
          <div class="pi pi-arrow-right" />
          <MemberAvatarGroup :members="participants" />
        </div>
        <div class="paid-by-text">
          <strong>
            <InlineGroupMember :member="paidBy" />
          </strong>
          <span>&nbsp;</span>
          paid for
        </div>
        <div class="description">
          {{ expense.description }}
        </div>
        <div class="amount-timestamp">
          <div class="amount">
            <strong>{{ formatCurrency(expense.amount) }}</strong>
          </div>
          <div class="timestamp">
            {{ formatDateTime(expense.timestamp) }}
          </div>
        </div>
      </template>
    </Card>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { Expense } from '#shared/types/api';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import MemberAvatarGroup from '~/components/common/MemberAvatarGroup.vue';
import { formatCurrency, formatDateTime } from '#shared/utils';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import type { Member } from '#shared/types/member';

const props = withDefaults(
  defineProps<{
    groupId?: string | null;
    expense: Expense;
    members?: Member[] | null;
  }>(),
  {
    groupId: null,
    members: null,
  },
);
const { groupId: pageGroupId, members: pageGroupMembers } = useDetailPageGroup();
const groupId = computed<string | null>(() => props.groupId ?? pageGroupId.value);
const members = computed<Member[] | null>(
  () => props.members ?? (pageGroupMembers.value as Member[]),
);
const paidBy = computed<Member | null>(() => getMember(props.expense.paidByMemberId));
const participants = computed<(Member | null)[]>(() =>
  props.expense.participants.map((p) => getMember(p.memberId)),
);

const getMember = (memberId: string): Member | null =>
  members.value?.find((m) => m.id === memberId) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.expense-card-link {
  @include utilities.reset-link;
}

:deep(.p-card-content) {
  @include utilities.flex-column(false);

  .members-avatars {
    @include utilities.flex-row-align-center;
    justify-content: center;
    margin-bottom: calc(var(--default-spacing) / 2);
  }

  .paid-by-text {
    display: inline-flex;
    align-items: center;
  }

  .description {
    font-size: 130%;
  }

  .amount-timestamp {
    @include utilities.flex-row-justify-between(false);
    align-items: flex-end; // put timestamp on same baseline as amount

    .amount {
      font-size: 110%;
    }

    .timestamp {
      font-size: 0.75rem;
      @include utilities.muted;
    }
  }
}
</style>
