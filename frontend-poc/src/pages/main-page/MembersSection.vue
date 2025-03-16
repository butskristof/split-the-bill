<template>
  <section>
    <h2>Members</h2>
    <ul>
      <li
        v-for="member in membersData?.members"
        :key="member.id"
      >
        {{ member.name }} ({{ member.id }})
      </li>
    </ul>
  </section>
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query';
import { ofetch } from 'ofetch';

const { data: membersData } = useQuery({
  queryKey: ['members', 'all'],
  queryFn: () => ofetch('/api/members'),
});
</script>
<style scoped></style>
