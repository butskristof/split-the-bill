<template>
  <div class="group-detail-page">
    <UButton
      icon="i-mynaui-arrow-left"
      variant="ghost"
      to="/groups"
      class="back-button"
      >Back to groups</UButton
    >

    <LoadingIndicator
      v-if="isPending"
      entity="group"
    />
    <ApiError
      v-else-if="isError"
      :error="error"
    />
    <div
      v-else
      class="group"
    >
      <h1>{{ group.name }}</h1>
      <div class="prose">
        <pre>{{ JSON.stringify(group, null, 2) }}</pre>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import ApiError from '~/components/common/ApiError.vue';

const route = useRoute();
const id = route.params.id as string;

const { getGroup } = useSplitTheBillApi();
const { data: group, isError, isPending, error } = getGroup(id);
</script>

<style scoped>
::v-deep(.back-button) {
  margin-bottom: 0.5rem;
}
</style>
