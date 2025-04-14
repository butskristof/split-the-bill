<template>
  <div class="header">
    <h2>Members</h2>
    <UButton
      label="Add"
      icon="i-mynaui-plus"
    />
  </div>
  <div class="member-list">
    <div
      v-for="member in members"
      :key="member.id"
      class="member-list-item"
    >
      <UAvatar
        :alt="member.name"
        class="avatar"
      />
      <h3 class="name">{{ member.name }}</h3>
      <UIcon name="i-mynaui-chevron-down" />
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  members: {
    id: string;
    name: string;
  }[];
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.header {
  @include utilities.flex-row-justify-between;
}

.member-list-item {
  @include utilities.flex-row-align-center;
  padding: var(--default-spacing);
  border-radius: var(--ui-radius);
  @include utilities.list-item-hover;
  @include utilities.list-item-separator;

  .name {
    flex-grow: 1;
  }

  &:hover {
    .avatar {
      background-color: var(--ui-bg-accented);
    }
  }
}
</style>
