<template>
  <AppLinkCard
    :to="{
      name: 'groups-id-payments-paymentId',
      params: {
        id: groupId,
        paymentId: payment.id,
      },
    }"
    class="payment-card"
  >
    <template #content>
      <div class="members-avatars">
        <MemberAvatar :member="sender" />
        <div class="pi pi-arrow-right" />
        <MemberAvatar :member="receiver" />
      </div>
      <p class="members-text">
        <strong>
          <InlineGroupMember :member="sender" />
        </strong>
        <span>&nbsp;</span>
        paid back
        <span>&nbsp;</span>
        <strong>
          <InlineGroupMember :member="receiver" />
        </strong>
      </p>
      <div class="amount-timestamp">
        <div class="amount">
          <strong>{{ formatCurrency(payment.amount) }}</strong>
        </div>
        <div class="timestamp">
          {{ formatDateTime(payment.timestamp) }}
        </div>
      </div>
    </template>
  </AppLinkCard>
</template>

<script setup lang="ts">
import AppLinkCard from '~/components/common/AppLinkCard.vue';
import type { Payment } from '#shared/types/api';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import { formatCurrency, formatDateTime } from '#shared/utils';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import type { Member } from '#shared/types/member';

const props = withDefaults(
  defineProps<{
    groupId?: string | null;
    payment: Payment;
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

const sender = computed<Member | null>(() => getMember(props.payment.sendingMemberId));
const receiver = computed<Member | null>(() => getMember(props.payment.receivingMemberId));

const getMember = (memberId: string): Member | null =>
  members.value?.find((m) => m.id === memberId) ?? null;
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.payment-card {
  :deep(.p-card-content) {
    @include utilities.flex-column(false);

    .members-avatars {
      @include utilities.flex-row-align-center;
      justify-content: center;
      margin-bottom: calc(var(--default-spacing) / 2);
    }

    .members-text {
      display: inline-flex;
      align-items: center;
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
}
</style>
