import Lara from '@primeuix/themes/lara';
import { definePreset } from '@primeuix/themes';

const theme = definePreset(Lara, {
  semantic: {
    primary: {
      50: '{emerald.50}',
      100: '{emerald.100}',
      200: '{emerald.200}',
      300: '{emerald.300}',
      400: '{emerald.400}',
      500: '{emerald.500}',
      600: '{emerald.600}',
      700: '{emerald.700}',
      800: '{emerald.800}',
      900: '{emerald.900}',
      950: '{emerald.950}',
    },
    colorScheme: {
      light: {
        surface: {
          0: '#ffffff',
          50: '{neutral.50}',
          100: '{neutral.100}',
          200: '{neutral.200}',
          300: '{neutral.300}',
          400: '{neutral.400}',
          500: '{neutral.500}',
          600: '{neutral.600}',
          700: '{neutral.700}',
          800: '{neutral.800}',
          900: '{neutral.900}',
          950: '{neutral.950}',
        },
      },
      dark: {
        surface: {
          0: '#ffffff',
          50: '#ececec',
          100: '#dedfdf',
          200: '#c4c4c6',
          300: '#adaeb0',
          400: '#97979b',
          500: '#7f8084',
          600: '#6a6b70',
          700: '#55565b',
          800: '#3f4046',
          900: '#2c2c34',
          950: '#16161d',
        },
      },
    },
  },
});

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: [
    '@nuxt/eslint',
    '@nuxt/fonts',
    'nuxt-security',
    '@primevue/nuxt-module',
    'nuxt-oidc-auth',
    'nuxt-open-fetch',
    '@regle/nuxt',
    '@nuxtjs/color-mode',
  ],
  security: {},
  components: {
    // disable auto-import of components
    dirs: [],
  },
  css: ['~/styles/reset.css', 'primeicons/primeicons.css', '~/styles/main.scss'],
  primevue: {
    options: {
      theme: {
        preset: theme,
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
    '/': {
      redirect: '/groups',
    },
  },
  openFetch: {
    clients: {
      backendApi: {
        baseURL: '/api/backend',
        schema: 'openapi/backend-api/spec.json',
      },
    },
  },
  runtimeConfig: {
    backendBaseUrl: '', // set in env
    redis: {
      host: '',
      port: 0,
      password: '',
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
});