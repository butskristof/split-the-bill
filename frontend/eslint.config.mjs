// @ts-check
import withNuxt from './.nuxt/eslint.config.mjs';
import pluginVueScopedCss from 'eslint-plugin-vue-scoped-css';
import pluginQuery from '@tanstack/eslint-plugin-query';

export default withNuxt(
  ...pluginVueScopedCss.configs['flat/recommended'],
  ...pluginQuery.configs['flat/recommended'],
  {
    name: 'app/custom-rules',
    rules: {
      'no-console': 'warn',
      'no-debugger': 'warn',
      'no-restricted-imports': [
        'error',
        {
          patterns: ['../*'],
        },
      ],
    },
  },
);
