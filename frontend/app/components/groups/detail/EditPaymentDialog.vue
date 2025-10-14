<template>
  <AppDialog
    :header="isCreate ? 'Add new payment' : 'Edit payment'"
    @close="tryClose"
  >
    <form @submit.prevent="onFormSubmit">
      <div class="amount-timestamp">
        <div class="form-field">
          <label for="amount">Amount</label>
          <InputNumber
            v-model="r$.$value.amount"
            input-id="amount"
            mode="currency"
            currency="EUR"
            :min="0.01"
            :min-fraction-digits="2"
            :max-fraction-digits="2"
            :invalid="r$.amount.$error"
            :disabled="formDisabled"
            autofocus
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
            v-model="r$.$value.timestamp"
            input-id="timestamp"
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

      <div class="sender-receiver">
        <div class="form-field">
          <label for="sender">Sender</label>
          <Select
            v-model="r$.$value.sender"
            input-id="sender"
            :options="props.members"
            option-label="name"
            :invalid="r$.sender.$error"
            :disabled="formDisabled"
            show-clear
          />
          <div
            v-if="r$.sender.$error"
            class="errors"
          >
            <Message
              v-for="error of senderErrors"
              :key="error"
              severity="error"
              size="small"
              variant="simple"
              >{{ error }}</Message
            >
          </div>
        </div>

        <div class="form-field">
          <label for="receiver">Receiver</label>
          <Select
            v-model="r$.$value.receiver"
            input-id="receiver"
            :options="props.members"
            option-label="name"
            :invalid="r$.receiver.$error"
            :disabled="formDisabled"
            show-clear
          />
          <div
            v-if="r$.receiver.$error"
            class="errors"
          >
            <Message
              v-for="error of receiverErrors"
              :key="error"
              severity="error"
              size="small"
              variant="simple"
              >{{ error }}</Message
            >
          </div>
        </div>
      </div>

      <div class="form-footer">
        <div class="messages">
          <Message
            v-if="isSuccess"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            {{ isCreate ? 'New payment created successfully' : 'Successfully saved changes' }}
          </Message>
          <Message
            v-if="apiErrorTitle"
            severity="error"
            variant="simple"
          >
            {{ apiErrorTitle }}
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
import { inferRules, createRule, type Maybe } from '@regle/core';
import {
  date,
  dateBefore,
  decimal,
  isFilled,
  minValue,
  oneOf,
  required,
  string,
  withMessage,
} from '@regle/rules';
import type { CreatePaymentRequest, Payment } from '#shared/types/api';
import { mapProblemDetailsErrorsToExternalErrors } from '#shared/utils';
import type { FetchError } from 'ofetch';
import { useCreatePaymentMutation } from '~/composables/backend-api/useCreatePaymentMutation';
import { useUpdatePaymentMutation } from '~/composables/backend-api/useUpdatePaymentMutation';
import type { Member } from '#shared/types/member';
import { DIALOG_SUCCESS_CLOSE_DELAY } from '#shared/constants';

const props = defineProps<{
  groupId: string;
  members: Member[];
  payment?: Payment | undefined;
}>();
const isCreate = computed(() => !props.payment);
const emit = defineEmits<{
  close: [];
}>();
const tryClose = () => {
  // should either not be touched by user or confirmed they want to close and lose changes
  if (
    isSuccess.value ||
    !r$.$anyDirty ||
    confirm('There are unsaved changes. Are you sure you want to close this dialog?')
  ) {
    emit('close');
  }
};

//#region form

const createMutation = useCreatePaymentMutation();
const updateMutation = useUpdatePaymentMutation();

const isSubmitting = computed(
  () => (isCreate.value ? createMutation : updateMutation).isPending.value,
);
const isSuccess = computed(
  () => (isCreate.value ? createMutation : updateMutation).isSuccess.value,
);

const formDisabled = computed<boolean>(() => isSubmitting.value || isSuccess.value);
const apiError = computed(() => (isCreate.value ? createMutation : updateMutation).error.value);

// map to a single string[]
const senderErrors = computed<string[]>(() =>
  // for now we only care about the messages, not the property they appeared on (only validation on
  // id for now)
  Object.entries(r$.sender.$errors).flatMap(([, v]) => v),
);
const receiverErrors = computed<string[]>(() =>
  Object.entries(r$.receiver.$errors).flatMap(([, v]) => v),
);
const apiErrorTitle = computed<string | null>(() => {
  if (!apiError.value) return null;
  const problemDetails = (apiError.value as FetchError)?.data as ProblemDetails;
  return problemDetails?.title ?? 'Something went wrong, please try again later.';
});
const externalErrors = computed<Record<string, string[]>>(() => {
  if (!apiError.value) return {};
  const problemDetails = (apiError.value as FetchError)?.data as ValidationProblemDetails;
  if (!problemDetails?.errors) return {};
  return {
    ...mapProblemDetailsErrorsToExternalErrors(problemDetails.errors),
    // property names in api request are different from the ones we're using in form state so we have
    // to map them back manually
    sender: problemDetails.errors['SendingMemberId'],
    receiver: problemDetails.errors['ReceivingMemberId'],
  };
});

// no payments in the future allowed
const maxTimestamp = new Date();

// Helper function to find member by ID
const findMemberById = (id: string): Member | null => {
  return props.members.find((m) => m.id === id) ?? null;
};

// defines actual form state, allows for invalid (empty) values so fields can be
// initialised empty or cleared
type FormState = {
  amount: number | null;
  timestamp: Date | null;
  sender: Member | null;
  receiver: Member | null;
};

const formState = ref<FormState>({
  amount: props.payment?.amount ?? null,
  timestamp: props.payment ? new Date(props.payment.timestamp) : maxTimestamp,
  sender: props.payment ? findMemberById(props.payment.sendingMemberId) : null,
  receiver: props.payment ? findMemberById(props.payment.receivingMemberId) : null,
});

// Computed array of valid member IDs for validation
const validMemberIds = computed(() => props.members.map((m) => m.id));

// Custom rule: validate that two member IDs are different
const differentMemberId = createRule({
  validator(value: Maybe<string>, otherMemberId: Maybe<string>) {
    // Only validate if both values are filled
    if (!isFilled(value) || !isFilled(otherMemberId)) return true;
    return value !== otherMemberId;
  },
  message: 'Sender and receiver must be different',
});

const formSchema = computed(() =>
  inferRules(formState, {
    // Amount: required, positive decimal (greater than 0)
    amount: {
      decimal,
      required,
      minValue: minValue(0, { allowEqual: false }),
    },

    // Timestamp: required, valid date, not in the future
    timestamp: {
      date,
      required,
      dateBefore: dateBefore(maxTimestamp, { allowEqual: true }),
    },

    // Sender: nested object, validate that id exists and is valid, and different from receiver
    sender: {
      id: {
        string,
        required,
        oneOf: withMessage(oneOf(validMemberIds.value), 'Invalid member selected'),
        differentMemberId: differentMemberId(() => formState.value.receiver?.id),
      },
    },

    // Receiver: nested object, validate that id exists and is valid, and different from sender
    receiver: {
      id: {
        string,
        required,
        oneOf: withMessage(oneOf(validMemberIds.value), 'Invalid member selected'),
        differentMemberId: differentMemberId(() => formState.value.sender?.id),
      },
    },
  }),
);

const { r$ } = useRegle(formState, formSchema, { externalErrors });

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  const request: CreatePaymentRequest = {
    groupId: props.groupId,
    sendingMemberId: data.sender.id,
    receivingMemberId: data.receiver.id,
    amount: data.amount,
    timestamp: data.timestamp.toISOString(),
  };
  if (isCreate.value) {
    createMutation.mutate(request, {
      onSuccess: () => {
        setTimeout(tryClose, DIALOG_SUCCESS_CLOSE_DELAY);
      },
    });
  } else {
    updateMutation.mutate(
      { ...request, id: props.payment!.id },
      {
        onSuccess: () => {
          setTimeout(tryClose, DIALOG_SUCCESS_CLOSE_DELAY);
        },
      },
    );
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

  .amount-timestamp,
  .sender-receiver {
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
