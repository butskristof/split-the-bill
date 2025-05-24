import Aura from '@primeuix/themes/aura';

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-05-15',
  future: {
    compatibilityVersion: 4,
  },
  devtools: { enabled: true },
  modules: [
    '@nuxt/eslint',
    '@nuxt/fonts',
    '@nuxtjs/color-mode',
    '@primevue/nuxt-module',
    '@nuxt/icon',
    '@vueuse/nuxt',
  ],
  css: ['~/assets/styles/reset.css', 'primeicons/primeicons.css', '~/assets/styles/main.scss'],
  components: {
    dirs: [],
  },
  primevue: {
    options: {
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.dark-mode',
        },
      },
    },
  },
  fonts: {
    defaults: {
      weights: [200, 300, 400, 500, 600, 700, 800, 900],
    },
  },
});