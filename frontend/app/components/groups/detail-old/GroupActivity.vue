<template>
  <div class="activity">
    <div class="description-amount">
      <div class="description">Dinner</div>
      <div class="amount">50,00 EUR</div>
    </div>
    <div class="row-2">
      <div class="paid-by-participants">
        <UTooltip text="Kristof">
          <UAvatar
            class="paid-by"
            alt="Kristof"
          />
        </UTooltip>
        <UIcon name="i-mynaui-arrow-right" />
        <UAvatarGroup class="participants">
          <UTooltip text="Alice">
            <UAvatar alt="Alice" />
          </UTooltip>
          <UTooltip text="Bob">
            <UAvatar alt="Bob" />
          </UTooltip>
          <UTooltip text="Charlie">
            <UAvatar alt="Charlie" />
          </UTooltip>
        </UAvatarGroup>
      </div>
      <div class="timestamp">10/04/2025 00:10</div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped>
@reference '~/assets/styles/main.css';

.activity {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  /*align-items: center;*/

  position: relative;
  margin-bottom: 1rem;
  padding-bottom: 1rem;

  .description-amount {
    display: flex;
    gap: 1rem;
    align-items: center;

    .description {
      flex-grow: 1;
    }
  }

  .row-2 {
    display: flex;
    gap: 1rem;
    align-items: center;
    justify-content: space-between;

    .paid-by-participants {
      display: flex;
      gap: 1rem;
      align-items: center;
    }

    .timestamp {
      @apply text-(--ui-text-muted);
    }
  }

  &:not(:last-child)::after {
    content: '';
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
    border-bottom: 1px solid var(--ui-border);
  }
}
</style>
