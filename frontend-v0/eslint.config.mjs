// @ts-check
import withNuxt from './.nuxt/eslint.config.mjs';
import pluginVueScopedCss from 'eslint-plugin-vue-scoped-css';

export default withNuxt(
  {
    name: 'app/files-to-ignore',
    ignores: [
      'node_modules/**',
      'dist/**',
      'dist-ssr/**',
      'coverage/**',
      '.vscode/**',
      '.idea/**',
      '*.min.js',
      'public/**',
      'build/**',
      '.nuxt/**',
      '.output/**',
      '*.md',
    ],
  },
  // @ts-expect-error
  ...pluginVueScopedCss.configs['flat/recommended'],
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
