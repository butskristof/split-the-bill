<template>
  <AppDialog>
    <form />
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

// no expenses in the future allowed
const maxTimestamp = new Date();

// defines actual form state, allows for invalid (empty) values so fields can be initialised empty
type FormState = {
  description: string;
  amount: number | undefined;
  timestamp: Date | undefined; // TODO check what the empty value is, undefined or null?
  paidBy: Member | null;
  participants: Member[];
};

const formState = ref<FormState>({
  description: '',
  amount: undefined,
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
        oneOf: oneOf(validMemberIds.value),
      },
    },

    // Participants: required, non-empty array with valid member IDs
    participants: {
      required,
      minLength: minLength(1),
      $each: {
        id: {
          string,
          required,
          oneOf: oneOf(validMemberIds.value),
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
