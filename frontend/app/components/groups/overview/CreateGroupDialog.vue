<template>
  <Dialog
    v-model:visible="visible"
    modal
    header="Create new group"
    :style="{ width: '95%', maxWidth: '750px' }"
  >
    <Form
      v-slot="$form"
      class="form"
      :initial-values="initialValues"
      :resolver="resolver"
      @submit="onSubmit"
    >
      <div class="form-field">
        <label for="name">Group name</label>
        <InputText
          id="name"
          name="name"
          type="text"
        />
        <Message
          v-if="$form.name?.invalid"
          size="small"
          severity="error"
          variant="simple"
          >{{ $form.name.error?.message }}</Message
        >
      </div>
      <div class="actions">
        <Button
          label="Cancel"
          severity="secondary"
          @click="visible = false"
        />
        <Button
          type="submit"
          label="Save"
          icon="pi pi-save"
        />
      </div>
    </Form>
  </Dialog>
</template>

<script setup lang="ts">
import { stringIsNullOrWhitespace } from '#shared/utils';

// pass through visible model so parent can control it like a bare Dialog
const visible = defineModel<boolean>('visible', { required: true });
const emit = defineEmits<{
  created: [];
}>();

// TODO warn when exiting when form is dirty

const initialValues = ref({
  name: '',
});
const resolver = ({ values }) => {
  const errors = {};
  if (stringIsNullOrWhitespace(values.name)) {
    errors.name = [{ message: 'Name is required' }];
  }

  return { values, errors };
};

const onSubmit = ({ valid }: { valid: boolean }) => {
  if (valid) {
    emit('created');
  }
};
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

.form {
  @include utilities.flex-column;

  .form-field {
    @include utilities.flex-column(false);
    gap: calc(var(--default-spacing) / 2);
  }

  .actions {
    @include utilities.flex-row-justify-end;
  }
}
</style>
