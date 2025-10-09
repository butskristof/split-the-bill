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
            v-model="r$.$value.amount"
            input-id="amount"
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

      <div class="form-field">
        <label for="paidBy">Paid by</label>
        <Select
          v-model="r$.$value.paidBy"
          input-id="paidBy"
          :options="[...props.members, { id: 'hey', name: 'Test' }]"
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
            v-for="error of paidByErrors"
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
        <Listbox
          id="participants"
          v-model="r$.$value.participants"
          :options="[...props.members, { id: 'hey', name: 'Test' }]"
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
            v-for="error in participantsErrors"
            :key="error"
            severity="error"
            size="small"
            variant="simple"
            >{{ error }}
          </Message>
        </div>
      </div>

      <div class="form-footer">
        <div class="messages">
          <Message
            v-if="isCreated"
            severity="success"
            variant="simple"
            icon="pi pi-check"
          >
            New expense created successfully!
          </Message>
        </div>
        <div class="actions">
          <Button
            label="Cancel"
            severity="secondary"
            :disabled="formDisabled"
            @click="() => tryClose(false)"
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
import { ExpenseSplitType } from '#shared/types/expense-split-type';
import { inferRules } from '@regle/core';
import {
  date,
  dateBefore,
  decimal,
  maxLength,
  minLength,
  minValue,
  oneOf,
  required,
  string,
} from '@regle/rules';
import type { CreateExpenseRequest } from '#shared/types/api';

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
const tryClose = (created: boolean = false) => {
  emit('close', created);
};

//#region form

// TODO replace these w/ declarative (computed) values from r$ and/or TanStack Query
const isSubmitting: boolean = false;
const isCreated: boolean = false;
// don't forget to switch back to .value when moving to computeds
const formDisabled = computed<boolean>(() => isSubmitting || isCreated);
// map to a single string[]
const paidByErrors = computed<string[]>(() =>
  // for now we only care about the messages, not the property they appeared on (only validation on
  // id for now to check)
  Object.entries(r$.paidBy.$errors).flatMap(([, v]) => v),
);
const participantsErrors = computed<string[]>(() => {
  // regle returns different formats for collection and individual item errors, we'll map to a
  // flat string[] for now,
  // example returns of r$.participants.$errors:
  /*
  {
    "$self": [
      "This field is required"
    ],
    "$each": []
  }
   */
  /*
  {
    "$self": [],
    "$each": [
      {
        "id": [
          "Invalid member selected"
        ],
        "name": []
      }
    ]
  }
   */

  return [
    ...r$.participants.$errors.$self,
    ...r$.participants.$errors.$each.flatMap((itemErrors) =>
      Object.entries(itemErrors).flatMap(([, v]) => v as string[]),
    ),
  ];
});

// no expenses in the future allowed
const maxTimestamp = new Date();

// defines actual form state, allows for invalid (empty) values so fields can be initialised empty
type FormState = {
  description: string;
  amount: number | null;
  timestamp: Date | null;
  paidBy: Member | null;
  participants: Member[];
};

const formState = ref<FormState>({
  description: '',
  amount: null,
  timestamp: maxTimestamp,
  paidBy: null,
  participants: [],
});

// Computed array of valid member IDs for validation
const validMemberIds = computed(() => props.members.map((m) => m.id));

const formSchema = computed(() =>
  inferRules(formState, {
    // Description: required, valid string (1-512 chars)
    description: {
      required,
      string,
      minLength: minLength(1),
      maxLength: maxLength(512),
    },

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

    // PaidBy: nested object, validate that id exists and is valid
    paidBy: {
      id: {
        string,
        required,
        oneOf: withMessage(oneOf(validMemberIds.value), 'Invalid member selected'),
      },
    },

    // Participants: required, non-empty array with valid member IDs
    participants: {
      required,
      $each: {
        id: {
          string,
          required,
          oneOf: withMessage(oneOf(validMemberIds.value), 'Invalid member selected'),
        },
      },
    },
  }),
);

const { r$ } = useRegle(formState, formSchema);

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  const request: CreateExpenseRequest = {
    groupId: props.groupId,
    description: data.description,
    amount: data.amount,
    timestamp: data.timestamp.toISOString(),
    paidByMemberId: data.paidBy.id,
    splitType: ExpenseSplitType.Evenly,
    participants: data.participants.map((p) => ({ memberId: p.id })),
  };
  console.log(request);
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
