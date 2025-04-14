<template>
  <div class="activity-list-item">
    <div class="row-1">
      <div
        v-if="expense"
        class="description"
      >
        <span v-if="expense.description">{{ expense.description }}</span>
      </div>
      <div class="amount">
        <span v-if="activity.amount">{{ formatCurrency(activity.amount) }}</span>
      </div>
    </div>
    <div class="row-2">
      <div
        v-if="expense"
        class="avatars"
      >
        <UTooltip text="Kristof">
          <UAvatar alt="Kristof" />
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
      <div
        v-if="payment"
        class="avatars"
      />
      <div class="timestamp">
        <span v-if="activity.timestamp">
          {{ formatDate(activity.timestamp) }}
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { formatCurrency } from '#shared/utilities/format-currency';
import { formatDate } from '#shared/utilities/format-date';
import type { components } from '#shared/api-clients/split-the-bill-api/spec';

type ExpenseDto = components['schemas']['GroupDto.ExpenseDto'];
type PaymentDto = components['schemas']['GroupDto.PaymentDto'];

const props = defineProps<{
  activity: ExpenseDto | PaymentDto;
}>();

const expense = computed<ExpenseDto | undefined>(() => props.activity as ExpenseDto);
const payment = computed<PaymentDto | undefined>(() => props.activity as PaymentDto);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.activity-list-item {
  @include utilities.flex-column;
  .row-1,
  .row-2 {
    @include utilities.flex-row-justify-between;
  }

  .row-2 {
    align-items: center;
  }
}

.avatars {
  @include utilities.flex-row-align-center;
  gap: calc(var(--default-spacing) / 2);
}
</style>
