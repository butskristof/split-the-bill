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
    'nuxt-oidc-auth',
  ],
  css: ['~/assets/styles/reset.css', 'primeicons/primeicons.css', '~/assets/styles/main.scss'],
  components: {
    // disable auto-import of components
    dirs: [],
  },
  primevue: {
    options: {
      theme: {
        preset: Aura,
        options: {
          // as set by nuxt color mode module
          darkModeSelector: '.dark-mode',
        },
      },
    },
  },
  fonts: {
    defaults: {
      // include all weights by default (it's hard to spot specific ones missing)
      weights: [200, 300, 400, 500, 600, 700, 800, 900],
    },
  },
  routeRules: {
    // route to the groups overview by default
    '/': { redirect: '/bff-test' },
  },
  oidc: {
    defaultProvider: 'oidc',
    providers: {
      oidc: {
        clientId: '', // set in env
        clientSecret: '', // set in env
        // IDP passes back a code which we can exchange for tokens
        responseType: 'code',
        grantType: 'authorization_code',
        authenticationScheme: 'header', // authenticate token request w/ header
        // responseMode: 'form_post',
        // @ts-expect-error is required but not defined in types by current library version
        openIdConfiguration: '', // set in env
        authorizationUrl: '', // set in env
        tokenUrl: '', // set in env
        userInfoUrl: '', // set in env
        logoutUrl: '', // set in env
        redirectUri: '', // set in env (local callback url: [HOST]/auth/oidc/callback )
        pkce: true, // additional protection against auth code interception
        state: true, // csrf protection
        nonce: true, // ID token replay attack protection
        // ensure tokens are valid
        validateAccessToken: true,
        validateIdToken: true,
        // do not expose tokens in user session
        exposeAccessToken: false,
        exposeIdToken: false,
        // make sure to include offline_access to get back refresh token
        scope: ['openid', 'profile', 'offline_access'],
        // limit to fields which are relevant for the application
        filterUserInfo: ['sub', 'name'],
      },
    },
    session: {
      automaticRefresh: true,
      expirationCheck: true,
      expirationThreshold: 60, // seconds
      cookie: {
        // https://docs.duendesoftware.com/bff/fundamentals/session/handlers/#choosing-between-samesitelax-and-samesitestrict
        // can't do strict since IdP will be hosted on other site
        sameSite: 'lax',
      },
    },
    middleware: {
      globalMiddlewareEnabled: true, // protect everything except /auth by default
    },
  },
  nitro: {
    storage: {
      oidc: {
        driver: 'redis',
        base: 'oidcstorage',
      },
    },
  },
  runtimeConfig: {
    backendBaseUrl: '', // set in env
  },
});
