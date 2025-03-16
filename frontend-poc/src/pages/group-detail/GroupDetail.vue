<template>
  <p v-if="isLoading">Loading...</p>
  <div
    v-else-if="group != null"
    class="page"
  >
    <h1>{{ group.name }}</h1>
    <div>
      Total expense amount: {{ group.totalExpenseAmount }}<br />
      Total payment amount: {{ group.totalPaymentAmount }}<br />
      Total balance: {{ group.totalBalance }}
    </div>
    <GroupExpenses :group="group" />
    <GroupPayments :group="group" />
    <GroupMembers :group="group" />
  </div>
  <p v-else>Group not found</p>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import { useQuery } from '@tanstack/vue-query';
import { ofetch } from 'ofetch';
import GroupExpenses from '@/pages/group-detail/GroupExpenses.vue';
import GroupPayments from '@/pages/group-detail/GroupPayments.vue';
import GroupMembers from '@/pages/group-detail/GroupMembers.vue';
import type { GroupDto } from '@/types/groups';

const route = useRoute();
const id = route.params.id;

const { data: group, isLoading } = useQuery<GroupDto>({
  queryKey: ['groups', id],
  queryFn: () => ofetch(`/api/groups/${id}`),
});
</script>

<style scoped></style>
