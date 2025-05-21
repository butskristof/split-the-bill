import Aura from '@primeuix/themes/aura';

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-05-15',
  future: {
    compatibilityVersion: 4,
  },
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@primevue/nuxt-module'],
  components: {
    dirs: [],
  },
  primevue: {
    autoImport: true,
    components: {
      prefix: 'Prime',
    },
    options: {
      theme: {
        preset: Aura,
      },
    },
  },
});
