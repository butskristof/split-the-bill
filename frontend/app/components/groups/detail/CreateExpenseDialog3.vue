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
          :options="[...props.members, { id: 1, name: 'test' }]"
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
          :options="[...props.members, { id: 'a', name: 'test' }]"
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
            v-for="error in r$.participants.$errors"
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
    <PreformattedText
      v-if="false"
      :value="r$"
    />
  </AppDialog>
</template>

<script setup lang="ts">
import AppDialog from '~/components/common/AppDialog.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';
import {
  date,
  dateBefore,
  decimal,
  maxLength,
  minLength,
  minValue,
  required,
  string,
} from '@regle/rules';
import { ExpenseSplitType } from '#shared/types/expense-split-type';

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

type CreateExpenseRequest = {
  groupId: string;
  description: string;
  amount: number;
  timestamp: string;
  paidByMemberId: string;
  splitType: ExpenseSplitType;
  participants: {
    memberId: string;
    percentualShare?: number;
    exactShare?: number;
  }[];
};

const maxTimestamp = new Date();
const formDisabled = false;
const isSubmitting = false;
const isCreated = false;
const generalError = '';

type CreateExpenseForm = {
  description: string;
  amount?: number;
  timestamp: Date;
  paidBy?: Member;
  participants: Member[];
};
const formState = ref<CreateExpenseForm>({
  description: '',
  amount: undefined,
  timestamp: maxTimestamp,
  paidBy: undefined,
  participants: [],
});
const formSchema = {
  description: { string, required, minLength: minLength(1), maxLength: maxLength(512) },
  amount: { decimal, required, minValue: minValue(0, { allowEqual: false }) },
  timestamp: { date, required, dateBefore: dateBefore(maxTimestamp, { allowEqual: true }) },
  paidBy: { id: { string, required } },
  participants: { required, minLength: minLength(1), $each: { id: { string, required } } },
};
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
