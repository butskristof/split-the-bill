// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-27',
  future: {
    compatibilityVersion: 4,
  },
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/ui'],
  css: ['~/assets/styles/main.css'],
  icon: {
    customCollections: [
      {
        prefix: 'stb',
        dir: '~/assets/icons',
      },
    ],
  },
  routeRules: {
    '/': { redirect: '/groups' },
  },
  runtimeConfig: {
    public: {
      splitTheBillApi: {
        baseUrl: '',
        accessToken: '',
      },
    },
  },
});
