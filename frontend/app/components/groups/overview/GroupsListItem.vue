<template>
  <Card class="groups-list-item">
    <template #title>
      <div class="group-name">Group name</div>
      <div class="member-count"><i class="pi pi-users" /> 12</div>
    </template>
    <template #footer>
      <div class="actions">
        <i class="pi pi-arrow-right" />
      </div>
    </template>
  </Card>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.groups-list-item {
  ::v-deep(.p-card-title) {
    @include utilities.flex-row-justify-between-align-center;

    .member-count {
      font-weight: initial;
      font-size: initial;
    }
  }
  ::v-deep(.p-card-footer) {
    @include utilities.flex-row-justify-end;
  }
}
</style>
