<template>
  <AppCard>
    <h3>{{ expense.description }}</h3>
    <PreformattedText :value="expense" />
  </AppCard>
</template>

<script setup lang="ts">
import AppCard from '~/components/common/AppCard.vue';
import type { Expense } from '~/types';
import PreformattedText from '~/components/common/PreformattedText.vue';

defineProps<{
  expense: Expense;
}>();
</script>

<style scoped lang="scss"></style>
