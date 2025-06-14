<template>
  <AppCard class="card">
    <div class="description-amount">
      <h3>{{ expense.description }}</h3>
      <h3>{{ formatCurrency(expense.amount) }}</h3>
    </div>
    <div class="paid-by-participants">
      <div class="paid-by">
        <MemberAvatar :member="paidByMember" />
      </div>
      <Icon
        name="prime:arrow-right"
        size="large"
      />
      <div class="participants">
        <AvatarGroup>
          <MemberAvatar
            v-for="participant in participants"
            :key="participant.id"
            :member="participant"
          />
        </AvatarGroup>
      </div>
    </div>
    <div class="timestamp-details">
      <div class="timestamp">
        {{ formatTimestamp(expense.timestamp) }}
      </div>
      <NuxtLink :to="{ name: 'groups-id-expenses-expenseId', params: { expenseId: expense.id } }">
        <Button
          icon="pi pi-arrow-right"
          icon-pos="right"
          label="Details"
          variant="text"
        />
      </NuxtLink>
    </div>
  </AppCard>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import type { Expense } from '~/types';
import { formatCurrency, formatTimestamp } from '#shared/utils';
import MemberAvatar from '~/components/common/MemberAvatar.vue';

const props = defineProps<{
  expense: Expense;
}>();
const { getMember } = useDetailPageGroup();
const paidByMember = computed(() => getMember(props.expense.paidByMemberId));
const participants = computed(() =>
  props.expense.participants.map((p: Expense['participants'][0]) => getMember(p.memberId)),
);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.description-amount {
  @include utilities.flex-row-justify-between-align-center;
}

.timestamp {
  font-size: var(--text-sm);
  color: var(--p-surface-500);
}

.paid-by-participants {
  @include utilities.flex-row-justify-between-align-center;
}

.timestamp-details {
  @include utilities.flex-row-justify-between-align-center;
}
</style>
