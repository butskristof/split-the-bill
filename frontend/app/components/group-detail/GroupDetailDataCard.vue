<template>
  <UCard
    variant="subtle"
    class="card"
  >
    <div class="card-content">
      <div class="icon-wrapper">
        <UIcon
          :name="icon"
          class="icon"
        />
      </div>
      <div class="title">{{ title }}</div>
      <div class="value">{{ formattedValue }}</div>
    </div>
  </UCard>
</template>

<script setup lang="ts">
import { formatCurrency } from '#shared/utilities/formatting';

const props = defineProps<{
  icon: string;
  title: string;
  value: number;
  currency: boolean;
}>();

const formattedValue = computed(() =>
  props.currency ? formatCurrency(props.value, 'EUR') : props.value,
);
</script>

<style scoped>
@reference '~/assets/css/main.css';

.card {
  /*@apply bg-(--ui-bg-elevated)/50 ring ring-(--ui-border) transition hover:bg-(--ui-bg-elevated) hover:ring-(--ui-border-accented);*/
  @apply transition hover:bg-(--ui-bg-elevated) hover:ring-(--ui-border-accented);
  width: 100%;
  @media (min-width: 641px) {
    width: 30%;
  }
}

.card-content {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;

  .icon-wrapper {
    @apply p-2.5 rounded-full bg-(--ui-primary)/10 ring ring-inset ring-(--ui-primary)/25;
    display: flex;
    align-items: center;
    justify-content: center;

    .icon {
      @apply size-5 text-(--ui-primary);
      flex-shrink: 0;
    }
  }

  .title {
    @apply text-(--ui-text-muted) text-xl;
    flex-grow: 1;
    margin-right: 1rem;
  }

  .value {
    @apply text-2xl font-bold;
    /*flex-basis: 100%;*/
    flex-shrink: 0;
  }
}
</style>
