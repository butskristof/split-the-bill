<template>
  <UModal
    v-model:open="isOpen"
    :title="`${isEdit ? 'Edit' : 'Add'} expense`"
    :ui="{ footer: 'justify-between' }"
  >
    <template #body>
      <UForm
        ref="form"
        class="form"
        :schema="schema"
        :state="state"
        @submit="onSubmit"
      >
        <UFormField
          label="Description"
          name="description"
          class="form-field"
        >
          <UInput
            v-model="state.description"
            class="input"
          />
        </UFormField>

        <UFormField
          label="Participants"
          name="participants"
        >
          <USelectMenu
            v-model="state.participants"
            multiple
            :items="groupMembers"
            value-key="id"
            class="input"
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
import * as v from 'valibot';
import type { FormSubmitEvent } from '@nuxt/ui';

const group = useInjectGroup();
const groupMembers = computed(
  () => group?.value?.members.map((m) => ({ id: m.id, label: m.name })) ?? [],
);

const isOpen = defineModel<boolean>();
const isEdit = ref<boolean>(false);
const form = useTemplateRef('form');

const schema = v.object({
  description: v.pipe(v.string(), v.nonEmpty()),
});
type Schema = v.InferOutput<typeof schema>;

const state = ref({
  description: '',
  paidByMemberId: '',
  amount: 0,
  splitType: null,
  participants: [],
});

const toast = useToast();
async function onSubmit(event: FormSubmitEvent<Schema>) {
  toast.add({ title: 'Success', description: 'The form has been submitted.', color: 'success' });
  console.log(event.data);
}
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.form {
  @include utilities.flex-column;

  .form-field {
    .input {
      width: 100%;
    }
  }
}
</style>
