<template>
  <div class="expense-detail">
    <AppBackButton
      :to="{ name: 'groups-id-activity', params: { id: groupId } }"
      :prioritize-previous-route="true"
    />

    <div
      v-if="expense"
      class="expense"
    >
      <div class="members-avatars">
        <MemberAvatar :member="paidBy" />
        <div class="pi pi-arrow-right" />
        <MemberAvatarGroup :members="participants" />
      </div>

      <div class="description">
        {{ expense.description }}
      </div>

      <p class="paid-by-text">
        paid for by
        <span>&nbsp;</span>
        <strong>
          <InlineGroupMember :member="paidBy" />
        </strong>
        <span>&nbsp;</span>
        and shared with
      </p>

      <div class="participants">
        <div class="participants-list">
          <div
            v-for="(participant, index) in participants"
            :key="participant?.id ?? `id-${index}`"
            class="participant"
          >
            <div class="member">
              <InlineGroupMember :member="participant" />
            </div>
            <div class="amount">
              <span v-if="amountPerParticipant">
                owes {{ formatCurrency(amountPerParticipant) }}
              </span>
              <span v-else>N/A</span>
            </div>
          </div>
        </div>
      </div>

      <Divider />

      <div class="details">
        <div class="detail-row">
          <span class="label">Total expense amount</span>
          <span class="value">{{ formatCurrency(expense.amount) }}</span>
        </div>

        <div class="detail-row">
          <span class="label">Date & time</span>
          <span class="value">{{ formatDateTime(expense.timestamp) }}</span>
        </div>
      </div>

      <div class="edit-delete-expense">
        <div class="edit">
          <Button
            label="Edit expense details"
            icon="pi pi-pen-to-square"
            variant="text"
            @click="openEditExpenseDialog"
          />
          <EditExpenseDialog
            v-if="showEditExpenseDialog"
            :group-id="groupId"
            :members="members!.map((m) => ({ id: m.id!, name: m.name! }))"
            :expense="expense"
            @close="closeEditExpenseDialog"
          />
        </div>
        <div class="delete">
          <Button
            label="Delete expense"
            icon="pi pi-trash"
            severity="danger"
            variant="text"
            @click="openDeleteExpenseDialog"
          />
          <DeleteExpenseDialog
            v-if="showDeleteExpenseDialog"
            :group-id="groupId"
            :expense="expense"
            @close="closeDeleteExpenseDialog"
          />
        </div>
      </div>
    </div>
    <Message
      v-else
      severity="warn"
      >Expense not found</Message
    >
  </div>
</template>

<script setup lang="ts">
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import MemberAvatarGroup from '~/components/common/MemberAvatarGroup.vue';
import { formatCurrency, formatDateTime } from '#shared/utils';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import DeleteExpenseDialog from '~/components/groups/detail/DeleteExpenseDialog.vue';
import type { Expense } from '#shared/types/api';
import type { Member } from '#shared/types/member';
import AppBackButton from '~/components/app/AppBackButton.vue';
import EditExpenseDialog from '~/components/groups/detail/EditExpenseDialog.vue';

const { groupId, expenses, members } = useDetailPageGroup();
const route = useRoute();
const expenseId = computed(() => route.params.expenseId as string);

const expense = computed<Expense | null>(
  () => (expenses.value?.find((p) => p.id === expenseId.value) as Expense) ?? null,
);
const amountPerParticipant = computed(() =>
  expense.value?.amount ? expense.value.amount / expense.value.participants.length : null,
);
const paidBy = computed<Member | null>(() =>
  expense.value?.paidByMemberId ? getMember(expense.value.paidByMemberId) : null,
);
const participants = computed(() =>
  expense.value?.participants ? expense.value.participants.map((p) => getMember(p.memberId)) : [],
);

const getMember = (memberId: string): Member | null =>
  (members.value?.find((m) => m.id === memberId) as Member) ?? null;

//#region edit expense dialog
const showEditExpenseDialog = ref(false);
const openEditExpenseDialog = () => (showEditExpenseDialog.value = true);
const closeEditExpenseDialog = () => (showEditExpenseDialog.value = false);
//#endregion

//#region delete expense dialog
const showDeleteExpenseDialog = ref(false);
const openDeleteExpenseDialog = () => (showDeleteExpenseDialog.value = true);
const closeDeleteExpenseDialog = () => (showDeleteExpenseDialog.value = false);
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.expense {
  @include utilities.flex-column;

  .members-avatars {
    @include utilities.flex-row-align-center;
    justify-content: center;
    margin-bottom: calc(var(--default-spacing) / 2);
  }

  .edit-delete-expense {
    margin-top: calc(var(--default-spacing));
    @include utilities.flex-row-justify-between-align-center;
    flex-wrap: wrap;
  }
}

.description {
  font-size: 130%;
}

.paid-by-text {
  display: inline-flex;
  align-items: center;
}

.participants {
  @include utilities.flex-column;
  .participants-list {
    @include utilities.flex-column;
    .participant {
      @include utilities.flex-row-justify-between;
    }
  }
}

.details {
  @include utilities.flex-column;

  .detail-row {
    @include utilities.flex-row-justify-between;

    .label {
      @include utilities.muted;
    }
  }
}
</style>
