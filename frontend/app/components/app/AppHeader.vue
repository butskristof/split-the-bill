<template>
  <div class="header-container">
    <header ref="header">
      <AppHeaderTitle class="app-title" />
      <Button
        class="nav-toggle"
        variant="text"
        severity="secondary"
        icon="pi pi-bars"
        @click="toggleDropdown"
      />
      <div
        v-show="showDropdown || atLeastLg"
        class="dropdown-menu"
      >
        <AppHeaderMenuItems class="menu-items" />
        <div class="actions-user-info">
          <AppHeaderActions
            v-if="false"
            class="actions"
          />
          <template v-if="loggedIn">
            <div
              v-if="false"
              class="separator"
            />
            <AppHeaderUserInfo class="user-info" />
          </template>
        </div>
      </div>
    </header>
  </div>
</template>

<script setup lang="ts">
import AppHeaderTitle from '~/components/app/AppHeaderTitle.vue';
import AppHeaderActions from '~/components/app/AppHeaderActions.vue';
import AppHeaderUserInfo from '~/components/app/AppHeaderUserInfo.vue';
import AppHeaderMenuItems from '~/components/app/AppHeaderMenuItems.vue';

const { loggedIn } = useOidcAuth();

const showDropdown = ref(false);
const toggleDropdown = () => {
  showDropdown.value = !showDropdown.value;
};
const closeDropdown = () => {
  showDropdown.value = false;
};

const header = ref<HTMLElement>();
onClickOutside(header, closeDropdown);

const breakpoints = useAppBreakpoints();
const atLeastLg = computed(() => breakpoints.lg.value);

const router = useRouter();
router.beforeEach(closeDropdown);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.header-container {
  // header always has a predefined height and should always still to the top
  height: var(--app-header-height);
  position: sticky;
  top: 0;
  z-index: 1;

  display: flex;

  background-color: var(--p-surface-0);
  border-bottom: 1px solid var(--p-surface-200);
  box-shadow: 0 1px 2px 0 rgb(0 0 0 / 0.05);

  @include utilities.dark-mode {
    background-color: var(--p-surface-900);
    border-bottom: 1px solid var(--p-surface-800);
  }
}

header {
  @include utilities.app-container;

  & {
    width: 100%;

    // vertically center the content inside the header
    @include utilities.flex-row-justify-between-align-center(false);
    gap: calc(var(--default-spacing) * 2);
    padding-inline: var(--default-spacing);
  }

  .app-title {
    flex-shrink: 0;
  }

  .nav-toggle {
    @include utilities.media-min-lg {
      display: none;
    }
  }

  .dropdown-menu {
    //background-color: red;
    background-color: var(--p-surface-0);

    position: absolute;
    //top: var(--app-header-height);
    left: 0;
    top: 100%;
    width: 100%;

    @include utilities.flex-column;

    .menu-items {
      flex-grow: 1;
    }

    .actions-user-info {
      @include utilities.flex-row-justify-end-align-center;

      .separator {
        width: 1px;
        height: 1.5rem;
        background-color: var(--p-surface-200);
        @include utilities.dark-mode {
          background-color: var(--p-surface-700);
        }

        @include utilities.media-min-lg {
          display: none;
        }
      }
    }

    @include utilities.media-max-lg {
      padding: var(--default-spacing);
      border-bottom: 1px solid var(--p-surface-200);
      box-shadow: 0 1px 2px 0 rgb(0 0 0 / 0.05);
    }

    @include utilities.media-min-lg {
      //display: none;
      position: static;
      @include utilities.flex-row-align-center;
    }

    @include utilities.dark-mode {
      background-color: var(--p-surface-900);
      border-color: var(--p-surface-800);
    }
  }
}
</style>
