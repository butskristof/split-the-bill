<template>
  <main v-if="group">
    <h1>activity</h1>
    <div class="expenses">
      <h2>Expenses</h2>
      <PreformattedText :value="expenses" />
    </div>
    <div class="payments">
      <h2>Payments</h2>
      <PreformattedText :value="payments" />
    </div>
  </main>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';

const { group } = useDetailPageGroup();
const expenses = computed(() => group.value?.expenses || []);
const payments = computed(() => group.value?.payments || []);
</script>

<style scoped lang="scss"></style>
