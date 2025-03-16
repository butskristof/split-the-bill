<template>
  <section>
    <h2>Groups</h2>
    <ul>
      <li
        v-for="group in groupsData?.groups"
        :key="group.id"
      >
        {{ group.name }} ({{ group.id }})
      </li>
    </ul>
  </section>
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query';
import { ofetch } from 'ofetch';

const { data: groupsData } = useQuery({
  queryKey: ['groups', 'all'],
  queryFn: () => ofetch('/api/groups'),
});
</script>

<style scoped></style>
