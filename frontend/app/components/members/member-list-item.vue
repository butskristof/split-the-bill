<template>
  <li>{{ member.name }} ({{ member.id }})</li>
</template>

<script setup lang="ts">
import type { SplitTheBillApiResponse } from '#open-fetch';

defineProps<{
  member: SplitTheBillApiResponse<'GetMembers'>['members'][0];
}>();
</script>
