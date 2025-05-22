import Aura from '@primeuix/themes/aura';

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-05-15',
  future: {
    compatibilityVersion: 4,
  },
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@primevue/nuxt-module', '@nuxt/fonts', '@nuxtjs/color-mode'],
  css: ['~/assets/styles/reset.css', '~/assets/styles/main.css', '~/assets/styles/base.scss'],
  components: {
    dirs: [],
  },
  primevue: {
    // autoImport: true,
    // components: {
    //   prefix: 'Prime',
    // },
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
      weights: [100, 200, 300, 400, 500, 600, 700, 800, 900],
      // styles: ['normal', 'italic'],
      // subsets: ['latin', 'latin-ext'],
    },
  },
});
