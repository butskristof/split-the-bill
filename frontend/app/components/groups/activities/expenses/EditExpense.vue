<template>
  <UModal
    v-model:open="isOpen"
    :title="`${isEdit ? 'Edit' : 'Add'} expense`"
    :ui="{ footer: 'justify-between' }"
  >
    <template #body>
      <UForm
        ref="form"
        :state="state"
        @submit="onSubmit"
      >
        <UFormField
          label="Email"
          name="email"
        >
          <UInput v-model="state.email" />
        </UFormField>

        <UFormField
          label="Password"
          name="password"
        >
          <UInput
            v-model="state.password"
            type="password"
          />
        </UFormField>
      </UForm>
    </template>

    <template #footer>
      <UButton
        label="Cancel"
        color="neutral"
        variant="outline"
        @click="isOpen = false"
      />

      <UButton
        label="Save"
        @click="form?.submit"
      />
    </template>
  </UModal>
</template>

<script setup lang="ts">
import type { FormSubmitEvent } from '@nuxt/ui';

const isOpen = defineModel<boolean>();
const isEdit = ref<boolean>(false);
const form = useTemplateRef('form');

const state = ref({
  email: '',
  password: '',
});

const toast = useToast();
async function onSubmit(event: FormSubmitEvent<unknown>) {
  toast.add({ title: 'Success', description: 'The form has been submitted.', color: 'success' });
  console.log(event.data);
}
</script>
