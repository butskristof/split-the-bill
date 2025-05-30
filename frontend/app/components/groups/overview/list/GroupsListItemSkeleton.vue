<template>
  <div class="list-item">
    <div class="left">
      <Skeleton
        shape="circle"
        size="2rem"
      />
      <Skeleton
        width="10rem"
        height="1.25rem"
      />
    </div>
    <div class="right">
      <i class="pi pi-arrow-right" />
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.list-item {
  @include utilities.flex-row-justify-between-align-center;
  margin-inline: calc(var(--default-spacing) * -1);
  padding: var(--default-spacing);
  &:not(:last-child) {
    border-bottom: 1px solid var(--p-surface-200);
  }

  .pi {
    color: var(--p-surface-300);
  }

  &:hover {
    transition: background 0.2s;

    background-color: var(--p-surface-100);
    @include utilities.dark-mode {
      background-color: var(--p-surface-800);
    }
  }

  @include utilities.dark-mode {
    border-bottom-color: var(--p-surface-700);
    .pi {
      color: var(--p-surface-700);
    }
  }

  .left {
    @include utilities.flex-row-justify-between-align-center;
  }
}
</style>
