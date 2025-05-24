<template>
  <div class="user-info">
    <Button
      variant="text"
      severity="secondary"
      aria-haspopup="true"
      aria-controls="user_info_overlay_menu"
      class="user-info-button"
      @click="toggle"
    >
      <Avatar
        shape="circle"
        :label="label"
      />
      <div class="user-name">{{ name }}</div>
    </Button>
    <Menu
      id="user_info_overlay_menu"
      ref="menu"
      :model="menuItems"
      :popup="true"
    >
      <template #item="{ item, props }">
        <router-link
          v-if="item.route"
          v-slot="{ href, navigate }"
          :to="item.route"
          custom
        >
          <a
            v-ripple
            :href="href"
            v-bind="props.action"
            class="menu-item-link"
            @click="navigate"
          >
            <span :class="item.icon" />
            <span class="ml-2">{{ item.label }}</span>
          </a>
        </router-link>
        <a
          v-else
          v-ripple
          :href="item.url"
          :target="item.target"
          v-bind="props.action"
          class="menu-item-link"
        >
          <span :class="item.icon" />
          <span class="ml-2">{{ item.label }}</span>
        </a>
      </template>
    </Menu>
  </div>
</template>

<script setup lang="ts">
import type { MenuItem } from 'primevue/menuitem';

const menu = ref();
const menuItems: MenuItem[] = [
  {
    label: 'Authentication info',
    icon: 'pi pi-lock',
    route: '/user/auth-info',
  },
  {
    label: 'Logout',
    icon: 'pi pi-sign-out',
    command: () => logout(),
  },
];

const toggle = (event: Event) => {
  menu.value.toggle(event);
};

const { user, logout } = useOidcAuth();
const name = computed<string | undefined>(() => user.value?.userInfo?.name as string);
const label = computed<string>(() => name.value?.charAt(0).toUpperCase() ?? '');
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities';

.user-info {
  @include utilities.flex-row-align-center(false);
  gap: calc(var(--default-spacing) / 2);

  .user-name {
    color: var(--p-surface-800);
    font-weight: var(--font-weight-medium);

    @include utilities.dark-mode {
      color: var(--p-surface-100);
    }
  }

  .user-info-button {
    padding: 0;
    &:hover {
      background-color: transparent;
    }
  }
}
</style>

<!-- menu item is teleported so can't work with scoped -->
<!-- overlay already has an ID though, so it's reasonable -->
<!-- eslint-disable-next-line vue-scoped-css/enforce-style-type -->
<style lang="scss">
@use '~/assets/styles/utilities';

#user_info_overlay_menu {
  .menu-item-link {
    @include utilities.flex-row-align-center(false);
    gap: calc(var(--default-spacing) * 0.75);
    padding: var(--p-menu-item-padding);
    line-height: 1;

    .pi {
      color: var(--p-surface-500);
      @include utilities.dark-mode {
        color: var(--p-surface-400);
      }
    }

    &:hover {
      .pi {
        color: var(--p-surface-900);
        @include utilities.dark-mode {
          color: var(--p-surface-0);
        }
      }
    }
  }
}
</style>
