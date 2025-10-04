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
          50: '{soho.50}',
          100: '{soho.100}',
          200: '{soho.200}',
          300: '{soho.300}',
          400: '{soho.400}',
          500: '{soho.500}',
          600: '{soho.600}',
          700: '{soho.700}',
          800: '{soho.800}',
          900: '{soho.900}',
          950: '{soho.950}',
        },
      },
    },
  },
});

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/fonts', 'nuxt-security', '@primevue/nuxt-module'],
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
      },
    },
  },
});
