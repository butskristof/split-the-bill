<template>
  <AppModal
    header="Create new group"
    @close="$emit('close')"
  >
    <Form
      v-slot="$form"
      class="form"
      :resolver="resolver"
      @submit="onFormSubmit"
    >
      <div class="form-group">
        <label for="name">Name</label>
        <InputText
          id="name"
          name="name"
          type="text"
          fluid
        />
        <Message
          v-if="$form.name?.invalid"
          severity="error"
          size="small"
          variant="simple"
          >{{ $form.name.error?.message }}
        </Message>
      </div>
      <div class="actions">
        <Button
          label="Cancel"
          icon="pi pi-times"
          severity="secondary"
          @click="$emit('close')"
        />
        <Button
          type="submit"
          icon="pi pi-save"
          label="Save"
        />
      </div>
    </Form>
  </AppModal>
</template>

<script setup lang="ts">
import * as v from 'valibot';
import { valibotResolver } from '@primevue/forms/resolvers/valibot';
import AppModal from '~/components/common/AppModal.vue';

const { $backendApi } = useNuxtApp();
const toast = useToast();

const emit = defineEmits<{
  close: [];
}>();

const resolver = valibotResolver(
  v.object({
    name: v.pipe(v.string(), v.minLength(1, 'Name is required')),
  }),
);

const onFormSubmit = async ({ valid, values }) => {
  if (valid) {
    const result = await $backendApi('/Groups', {
      method: 'POST',
      body: {
        name: values.name,
      },
    });
    console.log(result);
    toast.add({ severity: 'success', summary: 'Group created successfully' });
    emit('close');
  }
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.form {
  @include utilities.flex-column;
}

.form-group {
  @include utilities.flex-column(false);
  gap: calc(var(--default-spacing) / 4);

  label {
    font-weight: var(--font-weight-medium);
    color: var(--p-surface-900);
    @include utilities.dark-mode {
      color: var(--p-surface-0);
    }
  }
}

.actions {
  @include utilities.flex-row-justify-between-align-center;
  margin-top: var(--default-spacing);
}
</style>
