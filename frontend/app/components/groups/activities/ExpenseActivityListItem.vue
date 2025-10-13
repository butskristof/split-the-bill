<template>
  <Card class="expense">
    <template #content>
      <div class="card-content">
        <div class="members">
          <div class="paid-by">
            <MemberAvatar :member="paidBy" />
          </div>
          <div class="pi pi-arrow-right" />
          <div class="participants">
            <MemberAvatarGroup :members="participants" />
          </div>
        </div>
        <div class="description">
          <span>{{ expense.description }}</span>
        </div>
        <div class="amount-timestamp">
          <div class="amount">
            <strong> {{ expense.amount.toFixed(2) }}</strong> euro
          </div>
          <div class="timestamp">
            {{ new Date(expense.timestamp).toLocaleString() }}
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>

<script setup lang="ts">
import type { Expense } from '#shared/types/api';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import MemberAvatarGroup from '~/components/common/MemberAvatarGroup.vue';

type Member = {
  id: string;
  name: string;
};

const props = defineProps<{
  expense: Expense;
  members: Member[];
}>();
const paidBy = computed<Member | null>(() => getMember(props.expense.paidByMemberId));
const participants = computed<Member[]>(() =>
  props.expense.participants.map((p) => getMember(p.memberId)).filter((p) => p != null),
);

const getMember = (memberId: string): Member | null =>
  props.members?.find((m) => m.id === memberId) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.card-content {
  @include utilities.flex-column(false);
}

.description {
  font-size: 130%;
}

.members {
  @include utilities.flex-row-align-center;
  justify-content: center;
  margin-bottom: calc(var(--default-spacing) / 2);
}

.amount-timestamp {
  @include utilities.flex-row-justify-between(false);
  align-items: flex-end;
  flex-wrap: wrap;

  .amount {
    font-size: 110%;
  }

  .timestamp {
    font-size: 0.75rem;
    color: var(--p-surface-500);
  }
}
</style>
