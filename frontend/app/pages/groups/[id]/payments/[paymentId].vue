<template>
  <div class="payment-detail-page">
    <AppPageBackButton :to="{ name: 'groups-id-activity', params: { id: group.id } }" />
    <GroupPayment :payment />
    <div class="delete-payment">
      <Button
        icon="pi pi-trash"
        label="Delete payment"
        severity="danger"
        variant="text"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import GroupPayment from '~/components/groups/activities/payments/GroupPayment.vue';
import AppPageBackButton from '~/components/common/AppPageBackButton.vue';

const route = useRoute();
const paymentId: string = route.params.paymentId as string;

const { group } = useDetailPageGroup();
const payment = computed(() => group.value?.payments?.find((p) => p.id === paymentId));
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.payment-detail-page {
  @include utilities.flex-column;
}

.delete-payment {
  @include utilities.flex-row-justify-end;
}
</style>
