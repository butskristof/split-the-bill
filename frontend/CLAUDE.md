# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with the Nuxt 3 frontend codebase.

## OpenTelemetry Integration

The frontend includes OpenTelemetry instrumentation for distributed tracing:

### Configuration
OpenTelemetry is configured via environment variables:
- `OTEL_EXPORTER_OTLP_ENDPOINT`: OTLP endpoint (default: http://localhost:4317)
- `OTEL_SERVICE_NAME`: Service name (default: split-the-bill-frontend)
- `OTEL_SERVICE_VERSION`: Service version (default: 1.0.0)
- `OTEL_EXPORTER_OTLP_PROTOCOL`: Protocol type (http/protobuf for HTTP, grpc for gRPC)

### What's Traced
- **HTTP requests** to BFF routes (`/server/api/backend/*`)
- **Authentication flows** (session retrieval, token extraction)
- **Proxy requests** to .NET backend API
- **Redis operations** for session storage
- **Error handling** and exceptions

### Manual Tracing
The BFF route includes custom spans for:
- `bff.proxy_request`: Main request handler
- `bff.get_user_session`: Session validation
- `bff.get_access_token`: Token extraction
- `bff.proxy_to_backend`: Backend API calls

### Distributed Tracing
Correlation headers are automatically injected when proxying to the backend API to enable end-to-end trace correlation with the .NET Aspire services.

## Frontend-Specific Development Commands

### Development Server
```bash
# Install dependencies
npm install

# Start development server (if not using Aspire)
npm run dev
# Runs on http://localhost:3000

# Start with Aspire (recommended - includes backend)
cd ../api
dotnet run --project src/3-hosts/AppHost/AppHost.csproj
```

### Code Quality
```bash
# Type checking
npm run test:ts

# Linting
npm run lint:check      # Check for issues
npm run lint:fix        # Auto-fix issues

# Formatting
npm run format:check    # Check formatting
npm run format:fix      # Auto-format code

# Run all checks before committing
npm run lint:check && npm run format:check && npm run test:ts
```

### Building
```bash
# Production build
npm run build

# Preview production build
npm run preview
```

## Architecture & Patterns

### Project Structure
```
frontend/
├── app/                    # Main application code
│   ├── components/        # Vue components (auto-import disabled)
│   │   ├── app/          # Shell components (header, footer)
│   │   ├── common/       # Reusable components
│   │   ├── form/         # Form components
│   │   └── groups/       # Feature-specific components
│   ├── pages/            # File-based routing
│   ├── composables/      # Shared composition functions
│   ├── assets/           # SCSS styles and static assets
│   └── middleware/       # Route middleware
├── server/               # Nitro server (BFF)
│   ├── api/             # API routes
│   └── utils/           # Server utilities
├── shared/              # Shared types and utilities
└── openapi/             # OpenAPI specs
```

### Component Patterns
```vue
<!-- Standard component structure -->
<template>
  <div class="component-name">
    <!-- Template here -->
  </div>
</template>

<script setup lang="ts">
// Explicit imports (no auto-import)
import { ref, computed } from 'vue'
import type { Group } from '#open-fetch-schemas'

// Props with TypeScript
interface Props {
  groupId: string
}
const props = defineProps<Props>()

// Emits typed
const emit = defineEmits<{
  update: [value: string]
}>()

// Composables
const { data, status, error, refresh } = await useLazyBackendApi('group', {
  path: { groupId: props.groupId }
})
</script>

<style scoped lang="scss">
// Component-specific styles
</style>
```

### Data Fetching Patterns
```typescript
// Using the type-safe API client
const { data, status, error, refresh } = await useLazyBackendApi('groups');

// With parameters
const { data } = await useLazyBackendApi('group', {
  path: { groupId: '123' },
  query: { include: 'members' }
});

// Manual refresh after mutation
await $backendApi('createGroup', { body: newGroup });
await refresh(); // Refresh the list
```

### Form Handling
```vue
<script setup lang="ts">
import { Form } from '@primevue/forms';
import { valibotResolver } from '@primevue/forms/resolvers/valibot';
import * as v from 'valibot';

// Define validation schema
const schema = v.object({
  name: v.pipe(
    v.string(),
    v.trim(),
    v.nonEmpty('Name is required'),
    v.maxLength(50, 'Name too long')
  ),
  amount: v.pipe(
    v.number(),
    v.minValue(0.01, 'Must be positive')
  )
});

// Form submission
const onSubmit = async (values: v.InferOutput<typeof schema>) => {
  try {
    await $backendApi('createExpense', { body: values });
    toast.add({ severity: 'success', summary: 'Created!' });
    await navigateTo('/groups');
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Failed' });
  }
};
</script>

<template>
  <Form :resolver="valibotResolver(schema)" @submit="onSubmit">
    <FormField name="name">
      <FormFieldLabel>Name</FormFieldLabel>
      <InputText />
      <Message severity="error" />
    </FormField>
  </Form>
</template>
```

### Authentication Flow
```typescript
// Check auth status
const { user, loggedIn, login, logout } = useOidcAuth();

// Protected routes handled by middleware
// Tokens managed server-side only

// User info available as
interface OidcUser {
  sub: string;        // User ID
  nickname?: string;
  email?: string;
}
```

### State Management
```typescript
// Provide/Inject for parent-child data sharing
// Parent component
const group = ref<Group>();
provide('group', group);

// Child component
const group = inject<Ref<Group>>('group');

// No Pinia/Vuex - use provide/inject or props
```

### BFF (Backend for Frontend) Pattern
```typescript
// Server route handles auth and proxying
// /server/api/backend/[...].ts
export default defineEventHandler(async (event) => {
  // Extract auth token from session
  const token = await getUserAccessToken(event);
  
  // Forward to backend with auth
  return await $fetch('/api/v1' + path, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
});
```

## Styling Conventions

### SCSS Structure
```scss
// Use design tokens
.component {
  padding: var(--default-spacing);
  max-width: var(--app-container);
  
  // Use mixins for common patterns
  @include flex-column($gap: 1rem);
  @include media-max-sm { /* mobile styles */ }
  
  // Dark mode support
  @include dark-mode {
    background: var(--p-gray-900);
  }
}

// Common utilities available:
// - Layout: flex-row, flex-column, flex-center
// - Responsive: media-min-*, media-max-*
// - Components: app-card, icon-color
```

### PrimeVue Customization
```scss
// Override PrimeVue component styles
:deep(.p-button) {
  // Customizations
}

// Use PrimeVue design tokens
color: var(--p-primary-color);
background: var(--p-surface-100);
```

## Common Patterns

### Loading States
```vue
<template>
  <LoadingIndicator v-if="status === 'pending'" />
  <ApiError v-else-if="status === 'error'" :error="error" />
  <div v-else-if="data">
    <!-- Content -->
  </div>
</template>
```

### Error Handling
```typescript
// API errors automatically handled by ApiError component
<ApiError :error="error" />

// Toast notifications for user feedback
const toast = useToast();
toast.add({
  severity: 'error',
  summary: 'Error',
  detail: error.message
});
```

### Type Safety
```typescript
// Use generated types from OpenAPI
import type { Group, Member } from '#open-fetch-schemas';

// Define component types
interface Query<T> {
  data: Ref<T | null>;
  status: Ref<'idle' | 'pending' | 'success' | 'error'>;
  error: Ref<Error | null>;
  refresh: () => Promise<void>;
}
```

## Key Dependencies
- **Nuxt 3**: Meta-framework
- **Vue 3**: Composition API with TypeScript
- **PrimeVue**: UI component library
- **Valibot**: Schema validation
- **nuxt-oidc-auth**: OIDC authentication
- **nuxt-open-fetch**: Type-safe API client
- **VueUse**: Utility composables

## Common Pitfalls

1. **No auto-imports** - Must explicitly import components
2. **Server-only auth** - Never access tokens client-side
3. **CSRF protection** - API calls need `x-csrf: 1` header
4. **Lazy loading default** - Use `useLazyBackendApi` for non-blocking
5. **SCSS not Tailwind** - Use mixins and design tokens
6. **Type narrowing** - Check data exists before using

## Adding New Features

1. Create components in appropriate feature folder
2. Define TypeScript interfaces for props/emits
3. Use composables for data fetching
4. Implement forms with Valibot validation
5. Handle loading/error states consistently
6. Add SCSS styles using design tokens
7. Update OpenAPI types if backend changes