<template>
  <h2>Payments</h2>
  <div
    v-for="payment in payments"
    :key="payment.id"
    class="payment"
  >
    Sending member: {{ payment.sendingMemberId }} - Receiving member:
    {{ payment.receivingMemberId }}
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { GroupDto } from '@/types/groups';

const props = defineProps<{
  group: GroupDto;
}>();
const payments = computed(() => props.group.payments);
</script>

<style scoped></style>
