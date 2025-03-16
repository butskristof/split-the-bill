import pluginVue from 'eslint-plugin-vue';
import { defineConfigWithVueTs, vueTsConfigs } from '@vue/eslint-config-typescript';
import skipFormatting from '@vue/eslint-config-prettier/skip-formatting';
// @ts-expect-error vite-plugin-eslint does not correctly export its types...
import pluginVueScopedCss from 'eslint-plugin-vue-scoped-css';

// To allow more languages other than `ts` in `.vue` files, uncomment the following lines:
// import { configureVueProject } from '@vue/eslint-config-typescript'
// configureVueProject({ scriptLangs: ['ts', 'tsx'] })
// More info at https://github.com/vuejs/eslint-config-typescript/#advanced-setup

export default defineConfigWithVueTs(
  {
    name: 'app/files-to-lint',
    files: ['**/*.{ts,mts,tsx,vue,cjs,mjs}'],
  },

  {
    name: 'app/files-to-ignore',
    ignores: [
      '**/node_modules/**',
      '**/dist/**',
      '**/dist-ssr/**',
      '**/coverage/**',
      '**/.vscode/**',
      '**/.idea/**',
      '**/*.min.js',
      '**/public/**',
      '**/build/**',
    ],
  },

  pluginVue.configs['flat/recommended'],
  vueTsConfigs.recommended,
  pluginVueScopedCss.configs['flat/recommended'],
  skipFormatting,
  {
    languageOptions: {
      ecmaVersion: 'latest',
    },
    rules: {
      'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
      'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
      'no-restricted-imports': [
        'error',
        {
          patterns: ['../*'],
        },
      ],
      // 'vue/no-multiple-template-root': 'error',
    },
  },
);
