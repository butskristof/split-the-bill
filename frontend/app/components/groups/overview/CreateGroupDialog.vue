<template>
  <Dialog
    :visible="true"
    modal
    header="Create new group"
    :style="{ width: '95%', maxWidth: '750px' }"
    :draggable="false"
    @update:visible="onUpdateVisible"
  >
    <form @submit.prevent="onFormSubmit">
      <div class="form-field">
        <label for="name">Group name</label>
        <InputText
          id="name"
          v-model.trim="r$.$value.name"
          type="text"
          :invalid="r$.name.$error"
          autofocus
        />
        <Message
          v-for="error in r$.name.$errors"
          :key="error"
          severity="error"
          size="small"
          variant="simple"
          >{{ error }}</Message
        >
      </div>
      <div class="actions">
        <Button
          label="Cancel"
          severity="secondary"
          @click="onCancel"
        />
        <Button
          type="submit"
          label="Save"
          icon="pi pi-save"
        />
      </div>
    </form>
  </Dialog>
</template>

<script setup lang="ts">
import { maxLength, required } from '@regle/rules';

const emit = defineEmits<{
  close: [created: boolean];
}>();
const onUpdateVisible = (newValue: boolean) => {
  // visible false -> close dialog
  if (!newValue) emit('close', false);
};
const onCancel = () => {
  // TODO check form dirty and ask confirmation
  emit('close', false);
};

//#region form

const { r$ } = useRegle(
  { name: '' },
  {
    name: { required, maxLength: maxLength(512) },
  },
);

const onFormSubmit = async () => {
  const { valid, data } = await r$.$validate();
  if (!valid) return;

  try {
    console.log(data);
    // submit to api & map errors
    emit('close', true);
  } catch (error) {
    console.error(error);
  }
};
//#endregion
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';

form {
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
