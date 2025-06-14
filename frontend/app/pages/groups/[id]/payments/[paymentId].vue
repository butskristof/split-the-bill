<template>
  <GroupPayment :payment />
</template>

<script setup lang="ts">
import GroupPayment from '~/components/groups/activities/payments/GroupPayment.vue';

const route = useRoute();
const paymentId: string = route.params.paymentId as string;

const { group } = useDetailPageGroup();
const payment = computed(() => group.value?.payments?.find((p) => p.id === paymentId));
</script>

<style scoped lang="scss"></style>
