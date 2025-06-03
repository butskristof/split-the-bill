<template>
  <AppModal
    header="Create new group"
    @close="$emit('close')"
  >
    <Form
      v-slot="$form"
      class="form"
      :initial-values="initialValues"
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
import type { FormSubmitEvent } from '@primevue/forms';

const { $backendApi } = useNuxtApp();
const toast = useToast();

// always declare, even with empty strings to avoid default null (and ugly error messages)
const initialValues = {
  name: '',
};
const schema = v.object({
  name: v.pipe(v.string(), v.minLength(1, 'Name is required')),
});
type FormType = v.InferInput<typeof schema>;
const resolver = valibotResolver(schema);

defineEmits<{
  close: [];
}>();

const onFormSubmit = async ({ valid, values }: FormSubmitEvent<FormType>) => {
  if (!valid) return;

  const result = await $backendApi('/Groups', {
    method: 'POST',
    body: values,
  });
  toast.add({ severity: 'success', summary: 'Group created successfully' });
  await navigateTo({ name: 'groups-id', params: { id: result.id } });
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
