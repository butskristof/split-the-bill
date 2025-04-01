// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-27',
  future: {
    compatibilityVersion: 4,
  },
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/ui', 'nuxt-open-fetch'],
  css: ['~/assets/css/main.css'],
  icon: {
    customCollections: [
      {
        prefix: 'stb',
        dir: '~/assets/icons',
      },
    ],
  },
  openFetch: {
    clients: {
      splitTheBillApi: {
        baseURL: 'http://localhost:5222',
      },
    },
  },
  routeRules: {
    '/': { redirect: '/groups' },
  },
});
