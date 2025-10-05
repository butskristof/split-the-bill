<template>
  <AppDialog
    header="Add new expense"
    @close="tryClose"
  >
    <form @submit.prevent="onFormSubmit">
      <div class="form-field">
        <label for="description">Description</label>
        <InputText
          id="description"
          v-model.trim="r$.$value.description"
          type="text"
          :invalid="r$.description.$error"
          :disabled="formDisabled"
          autofocus
        />
        <div
          v-if="r$.description.$error"
          class="errors"
        >
          <Message
            v-for="error in r$.description.$errors"
            :key="error"
            severity="error"
            size="small"
            variant="simple"
            >{{ error }}</Message
          >
        </div>
      </div>

      <div class="amount-timestamp">
        <div class="form-field">
          <label for="amount">Amount</label>
          <InputNumber
            id="amount"
            v-model="r$.$value.amount"
            mode="currency"
            currency="EUR"
            :min="0.01"
            :min-fraction-digits="2"
            :max-fraction-digits="2"
            :invalid="r$.amount.$error"
            :disabled="formDisabled"
          />
          <div
            v-if="r$.amount.$error"
            class="errors"
          >
            <Message
              v-for="error in r$.amount.$errors"
              :key="error"
              severity="error"
              size="small"
              variant="simple"
              >{{ error }}</Message
            >
          </div>
        </div>

        <div class="form-field">
          <label for="timestamp">Date and time</label>
          <DatePicker
            id="timestamp"
            v-model="r$.$value.timestamp"
            date-format="dd/mm/yy"
            show-time
            hour-format="24"
            :max-date="maxTimestamp"
            show-button-bar
            :invalid="r$.timestamp.$error"
            :disabled="formDisabled"
            fluid
            update-model-type="date"
          />
          <div
            v-if="r$.timestamp.$error"
            class="errors"
          >
            <Message
              v-for="error in r$.timestamp.$errors"
              :key="error"
              severity="error"
              size="small"
              variant="simple"
              >{{ error }}</Message
            >
          </div>
        </div>
      </div>

      <div class="form-field">
        <label for="paidBy">Paid by</label>
        <Select
          id="paidBy"
          v-model="r$.$value.paidBy"
          :options="props.members"
          option-label="name"
          :invalid="r$.paidBy.$error"
          :disabled="formDisabled"
          show-clear
        />
        <div
          v-if="r$.paidBy.$error"
          class="errors"
        >
          <Message
            v-for="error in r$.paidBy.$errors"
            :key="error"
            severity="error"
            size="small"
            variant="simple"
            >{{ error }}</Message
          >
        </div>
      </div>

      <div class="form-field">
        <label for="participants">Participants</label>
        <ListBox
          id="participants"
          v-model="r$.$value.participants"
          :options="props.members"
          option-label="name"
          multiple
          :invalid="r$.participants.$error"
          :disabled="formDisabled"
          fluid
        />
        <div
          v-if="r$.participants.$error"
          class="errors"
        >
          <Message
            v-for="error in r$.participants.$errors.$self"
            :key="error"
            severity="error"
            size="small"
            variant="simple"
            >{{ error }}</Message
          >
          <template
            v-for="(item, index) in r$.participants.$each"
            :key="index"
          >
            <Message
              v-for="error in item.id.$errors"
              :key="`${index}-${error}`"
              severity="error"
              size="small"
              variant="simple"
              >{{ error }}</Message
            >
          </template>
        </div>
      </div>

      <div class="form-footer">
        <div class="message">
          <Message
            v-if="isCreated"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            New expense created successfully!
          </Message>
          <Message
            v-if="generalError"
            severity="error"
            variant="simple"
          >
            {{ generalError }}
          </Message>
        </div>
        <div class="actions">
          <Button
            label="Cancel"
            severity="secondary"
            :disabled="formDisabled"
            @click="tryClose"
          />
          <Button
            type="submit"
            icon="pi pi-save"
            :loading="isSubmitting"
            :disabled="formDisabled"
            :label="isSubmitting ? 'Saving...' : 'Save'"
          />
        </div>
      </div>
    </form>
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import type { components } from '#open-fetch-schemas/backend-api';
import { date, dateBefore, maxLength, minLength, minValue, required } from '@regle/rules';
import { createRule } from '@regle/core';
import { mapProblemDetailsErrorsToExternalErrors } from '#shared/utils';

type Member = {
  id: string;
  name: string;
};

const props = defineProps<{
  groupId: string;
  members: Member[];
}>();
const emit = defineEmits<{
  close: [created: boolean];
}>();
const tryClose = () => {
  // should either not be touched by user or confirmed they want to close and lose changes
  if (
    isCreated.value ||
    !r$.$anyDirty ||
    confirm('There are unsaved changes. Are you sure you want to close this dialog?')
  ) {
    emit('close', isCreated.value);
  }
};

//#region form
const { $backendApi } = useNuxtApp();
type CreateExpenseRequest = components['schemas']['CreateExpense.Request'];
type ValidationProblemDetails = components['schemas']['HttpValidationProblemDetails'];

const externalErrors = ref<Record<string, string[]>>({});
const generalError = ref<string>();
const isCreated = ref(false);
const isSubmitting = ref(false);
const formDisabled = computed(() => isSubmitting.value || isCreated.value);

const maxTimestamp = new Date();

const isMember = createRule({
  validator(value: unknown, members: { id: string }[]) {
    if (!value) return false;

    let id: string;
    if (typeof value === 'string') {
      id = value;
    } else if (typeof value === 'object' && 'id' in value) {
      id = (value as { id: string }).id;
    } else {
      return false;
    }

    return members.some((m) => m.id === id);
  },
  message: 'Invalid member selected',
});

const { r$ } = useRegle(
  {
    description: '',
    amount: undefined,
    timestamp: maxTimestamp,
    paidBy: undefined,
    participants: [...props.members],
  },
  {
    description: { required, maxLength: maxLength(512) },
    amount: { required, minValue: minValue(0.01) },
    timestamp: { required, date, dateBefore: dateBefore(maxTimestamp) },
    paidBy: { required, isMember: isMember(props.members) },
    participants: {
      required,
      minLength: minLength(1),
      $each: {
        id: { isMember: isMember(props.members) },
      },
    },
  },
  { externalErrors },
);

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  // Clear previous errors
  generalError.value = undefined;
  isSubmitting.value = true;

  try {
    const requestBody: CreateExpenseRequest = {
      description: data.description,
      amount: data.amount,
      groupId: props.groupId,
      timestamp: data.timestamp.toISOString(),
      paidByMemberId: data.paidBy!['id'],
      participants: data.participants.map((p) => ({ memberId: p.id })),
      splitType: 0,
    };
    await $backendApi('/Groups/{groupId}/expenses', {
      method: 'POST',
      path: {
        groupId: props.groupId,
      },
      body: requestBody,
    });

    isCreated.value = true;
  } catch (error) {
    console.error(error);

    const errorData = (error as { data?: ValidationProblemDetails })?.data;

    if (errorData?.status === 400 && errorData.errors) {
      externalErrors.value = mapProblemDetailsErrorsToExternalErrors(errorData.errors);
    }

    if (errorData?.title) {
      // Handle other API errors (500, 404, etc.) with title
      generalError.value = errorData.title;
    } else {
      // Fallback for unknown errors
      generalError.value = 'Something went wrong. Please try again.';
    }
  } finally {
    isSubmitting.value = false;
  }
};
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

form {
  @include utilities.flex-column;

  .form-field {
    @include utilities.flex-column(false);
    gap: calc(var(--default-spacing) / 2);
  }

  .amount-timestamp {
    @include utilities.flex-row;
    flex-wrap: wrap;

    .form-field {
      flex: 1 1 0;
      min-width: calc(var(--default-spacing) * 12);
    }
  }

  .form-footer {
    @include utilities.flex-row-justify-between;
    flex-wrap: wrap;

    .actions {
      @include utilities.flex-row-justify-end;
      margin-left: auto;
    }
  }
}
</style>
