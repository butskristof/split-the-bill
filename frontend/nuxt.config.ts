// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/fonts', 'nuxt-security'],
  security: {},
  components: {
    // disable auto-import of components
    dirs: [],
  },
  css: ['~/styles/reset.css'],
});
