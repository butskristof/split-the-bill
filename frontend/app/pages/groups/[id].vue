<template>
  <div class="group-detail-page">
    <UButton
      icon="i-mynaui-arrow-left"
      variant="ghost"
      to="/groups"
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
      <div class="cards">
        <GroupDetailDataCard
          icon="i-mynaui-dollar"
          title="Expenses"
          :value="3000"
          :currency="true"
        />
        <GroupDetailDataCard
          icon="i-mynaui-dollar"
          title="Payments"
          :value="1000"
          :currency="true"
        />
      </div>
      <GroupDetailActivity />

      <pre>{{ JSON.stringify(group, null, 2) }}</pre>
    </div>
  </div>
</template>
<script setup lang="ts">
import ApiError from '~/components/common/ApiError.vue';
import LoadingIndicator from '~/components/common/LoadingIndicator.vue';
import GroupDetailDataCard from '~/components/group-detail/GroupDetailDataCard.vue';
import GroupDetailActivity from '~/components/group-detail/GroupDetailActivity.vue';

const route = useRoute();
const id = route.params.id;

const { getGroup } = useSplitTheBillApi();
const { data: group, isError, isPending, error } = getGroup(id as string);
</script>

<style scoped>
@reference '~/assets/css/main.css';

h1 {
  @apply text-4xl font-bold;
  margin-top: 1rem;
  margin-bottom: 2rem;
}

.cards {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin-bottom: 1rem;

  @media (min-width: 641px) {
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-between;
  }
}
</style>
