<template>
  <div class="payments">
    <h2>Payments</h2>
    <table>
      <thead>
        <tr>
          <th>Sender</th>
          <th>Receiver</th>
          <th>Amount</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="payment in payments"
          :key="payment.id"
        >
          <td>{{ memberNames[payment.sendingMemberId] }}</td>
          <td>{{ memberNames[payment.receivingMemberId] }}</td>
          <td>{{ payment.amount }}</td>
        </tr>
      </tbody>
    </table>
    <div class="add">
      <div
        v-if="!showAddForm"
        class="button"
      >
        <button @click="showAddForm = true">Add payment</button>
      </div>
      <div
        v-if="showAddForm"
        class="form"
      >
        <div class="form-row">
          <label for="sendingMemberId">Sender</label>
          <select
            id="sendingMemberId"
            v-model="sendingMemberId"
          >
            <option
              v-for="(name, id) in memberNames"
              :key="id"
              :value="id"
            >
              {{ name }}
            </option>
          </select>
        </div>
        <div class="form-row">
          <label for="receivingMemberId">Receiver</label>
          <select
            id="receivingMemberId"
            v-model="receivingMemberId"
          >
            <option
              v-for="(name, id) in memberNames"
              :key="id"
              :value="id"
            >
              {{ name }}
            </option>
          </select>
        </div>
        <div class="form-row">
          <label for="amount">Amount</label>
          <input
            id="amount"
            v-model.number="amount"
            type="number"
          />
        </div>
        <div class="form-actions">
          <button @click="mutate">Save</button>
          <button @click="showAddForm = false">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import type { GroupDto } from '@/types/groups';
import { useMutation } from '@tanstack/vue-query';
import { ofetch } from 'ofetch';

const props = defineProps<{
  group: GroupDto;
}>();
const payments = computed(() => props.group.payments);

const memberNames = computed(() =>
  props.group.members.reduce(
    (result, member) => {
      result[member.id] = member.name;
      return result;
    },
    {} as Record<string, string>,
  ),
);

//#region add
const showAddForm = ref(false);
const sendingMemberId = ref(null);
const receivingMemberId = ref(null);
const amount = ref(0);

const { mutate } = useMutation({
  onMutate: () =>
    ofetch(`/api/groups/${props.group.id}/payments`, {
      method: 'POST',
      body: JSON.stringify({
        sendingMemberId: sendingMemberId.value,
        receivingMemberId: receivingMemberId.value,
        amount: amount.value,
      }),
      headers: {
        'Content-Type': 'application/json',
      },
    }),
  onError: (e) => {
    console.error(e);
  },
});
//#endregion
</script>

<style scoped>
.payments {
  margin-bottom: 1rem;
}
table {
  border-collapse: collapse;
}

th,
td {
  border: 1px solid black;
  padding-left: 0.5rem;
  padding-right: 0.5rem;
}
</style>
