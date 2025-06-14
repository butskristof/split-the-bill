<template>
  <AppCard class="card">
    <div class="amount">
      <h3>{{ formatCurrency(payment.amount) }}</h3>
    </div>
    <div class="avatars">
      <MemberAvatar :member="sendingMember" />
      <Icon
        name="prime:arrow-right"
        size="large"
      />
      <MemberAvatar :member="receivingMember" />
    </div>
    <div class="timestamp-details">
      <div class="timestamp">
        {{ formatTimestamp(payment.timestamp) }}
      </div>
      <NuxtLink :to="{ name: 'groups-id-payments-paymentId', params: { paymentId: payment.id } }">
        <Button
          icon="pi pi-arrow-right"
          icon-pos="right"
          label="Details"
          variant="text"
        />
      </NuxtLink></div
  ></AppCard>
</template>

<script setup lang="ts">
import type { Payment } from '~/types';
import MemberAvatar from '~/components/common/MemberAvatar.vue';
import AppCard from '~/components/common/AppCard.vue';
import { formatCurrency, formatTimestamp } from '#shared/utils';

const props = defineProps<{
  payment: Payment;
}>();
const { getMember } = useDetailPageGroup();
const sendingMember = computed(() => getMember(props.payment.sendingMemberId));
const receivingMember = computed(() => getMember(props.payment.receivingMemberId));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.amount {
  @include utilities.flex-row-justify-end;
}

.avatars {
  @include utilities.flex-row-justify-between-align-center;
}

.timestamp {
  font-size: var(--text-sm);
  color: var(--p-surface-500);
}

.timestamp-details {
  @include utilities.flex-row-justify-between-align-center;
}
</style>
