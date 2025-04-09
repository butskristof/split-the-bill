<template>
  <div class="activities">
    <div class="header">
      <h2>Activities</h2>
      <div class="actions">
        <UButton
          icon="i-mynaui-plus"
          label="Add"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped>
@reference '~/assets/css/main.css';

.header {
  display: flex;
  justify-content: space-between;
  /*align-items: center;*/
  margin-bottom: 1rem;

  h2 {
    @apply text-xl font-bold;
  }
}
</style>
