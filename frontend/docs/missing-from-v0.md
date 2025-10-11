# Missing Features from frontend-v0

This document outlines features, components, and functionality present in `frontend-v0` that are missing in the current `frontend` implementation.

## 1. Group Management Features

### Delete Group Functionality
Complete feature missing from new version:
- `DeleteGroup.vue` component - Delete button with modal trigger
- `DeleteGroupModal.vue` component - Confirmation modal with error handling
- Delete API integration
- Navigation after successful deletion

**Location in v0:** `app/components/groups/edit/`

## 2. UI/UX Components

### Loading & Error States
- **`LoadingIndicator.vue`** - Simple spinner component (`pi pi-spin pi-spinner`)
- **`PageLoadingIndicator.vue`** - Full page loading with optional message/entity text
- **`ApiError.vue`** - Comprehensive error display component with:
  - Error icon and title
  - User-friendly message
  - Collapsible technical details toggle
  - Styled error card with ProblemDetails support
- **`GroupsListItemSkeleton.vue`** - Loading skeleton for list items

**Location in v0:** `app/components/common/`

### Navigation
- **`AppPageBackButton.vue`** - Styled back button with:
  - "Back to overview" label
  - Left arrow icon
  - Custom hover/focus styles
  - Negative margin positioning

**Location in v0:** `app/components/common/`

### Layout Components
- **`AppShell.vue`** - Main app layout wrapper with:
  - Min-height viewport fill
  - Flex column layout
  - AppHeader and AppFooter composition
  - Content area with proper spacing
  - Background color theming
- **`AppContainer.vue`** - Content container with max-width constraint
- **`AppCard.vue`** - Reusable card component with:
  - Configurable HTML tag (via `tag` prop)
  - SCSS mixin-based styling
- **`AppModal.vue`** - Standardized modal wrapper (used instead of `AppDialog.vue`) with:
  - PrimeVue Dialog wrapper
  - Responsive width handling using breakpoints
  - Auto-close on outside click
  - Emit close event

**Location in v0:** `app/components/app/` and `app/components/common/`

### Other Components
- **`MemberAvatar.vue`** - Avatar component with:
  - Tooltip showing member name
  - First letter extraction
  - Circle shape, large size
- **`ColorModeButtonHeadless.vue`** - Dark mode toggle with:
  - Headless component pattern (slot-based)
  - Light/dark/unknown states
  - Toggle and set functions
  - Dynamic icon (sun/moon)
  - Client-only rendering

**Location in v0:** `app/components/common/` and `app/components/app/`

## 3. Form Components

- **`FormFieldLabel.vue`** - Standardized form label with:
  - `htmlFor` prop for accessibility
  - `required` prop support
  - Integration with FormFieldLabelRequiredIndicator
  - Medium font-weight styling
- **`FormFieldLabelRequiredIndicator.vue`** - Required field asterisk with:
  - Red color matching PrimeVue error messages
  - Dark mode support

**Location in v0:** `app/components/form/`

## 4. Header Components (More Granular Architecture)

The v0 version has a sophisticated, component-based header structure:

- **`AppHeader.vue`** - Main header container with:
  - Sticky positioning
  - Responsive hamburger menu toggle
  - Dropdown menu for mobile
  - Outside click handling
  - Route change menu closing
  - Breakpoint-aware display
- **`AppHeaderTitle.vue`** - Separate title component
- **`AppHeaderActions.vue`** - Actions container (placeholder for future features)
- **`AppHeaderUserInfo.vue`** - Complex user menu with:
  - Avatar with user initial
  - User name display
  - Dropdown menu (PrimeVue Menu)
  - Menu items:
    - Authentication info link
    - Logout action
  - Custom styled menu items
  - Router link integration
- **`AppHeaderMenuItems.vue`** - Navigation menu items component

**Features in v0 header:**
- Responsive hamburger menu for mobile devices
- User authentication display with dropdown
- Avatar with user initial
- Proper z-index layering
- Border and shadow styling
- Dark mode support throughout

**Location in v0:** `app/components/app/header/`

**Current implementation:** Much simpler `AppHeader.vue` with just title and empty actions div

## 5. Footer Components (More Detailed Structure)

- **`AppFooter.vue`** - More detailed footer with proper structure
- **`AppFooterTitle.vue`** - Separate footer title component

**Location in v0:** `app/components/app/footer/`

## 6. Dependencies & Libraries

### Missing from New Version
- **`@nuxtjs/color-mode`** - Dark mode support and color scheme management
- **`@nuxt/icon`** - Icon component with multiple icon sets
- **`@vueuse/nuxt`** - VueUse utilities (breakpoints, click outside, etc.)
- **`@primevue/forms`** - PrimeVue forms integration with resolvers

### Added in New Version (Not in v0)
- **`@regle/core`, `@regle/nuxt`, `@regle/rules`, `@regle/schemas`** - Form validation library
- **`@tanstack/vue-query`** - Data fetching and state management
- **`nuxt-security`** - Security headers and protections module

## 7. Composables

- **`useAppBreakpoints.ts`** - Responsive breakpoint utilities with:
  - Tailwind breakpoint preset integration
  - VueUse breakpoints wrapper
  - Exposed breakpoint values for programmatic use
  - Used by AppModal and AppHeader for responsive behavior

**Location in v0:** `app/composables/`

## 8. Type Definitions & Enums

### app/types.d.ts
Centralized type definitions including:
- `Query<T>` - Type for async data queries from useLazyBackendApi
- `ProblemDetails` - Error response type
- `ValidationProblemDetails` - Validation error type
- `Group` - Group entity from API
- `Expense` - Expense with type discriminator
- `Payment` - Payment with type discriminator
- `Activity` - Union of Expense and Payment
- `GroupMember` - Group member entity

**Location in v0:** `app/types.d.ts`

### app/enums.ts
- `ExpenseSplitType` enum (v0 uses traditional enum, new version uses const object pattern)

**Location in v0:** `app/enums.ts`

### Type Organization Differences
- **v0:** Uses `shared/common/types.d.ts` (just ProblemDetails)
- **New:** Uses `shared/types/api.ts` with more extensive type definitions

## 9. Page Structure Differences

### Group Detail Route
- **v0:** Nested routing pattern
  - `groups/[id].vue` - Layout page with loading/error handling and PageLoadingIndicator
  - `groups/[id]/index.vue` - Content page with GroupDetail component and AppPageBackButton
  - Uses `<NuxtPage>` for nested routing
  - Provides group data to children via `useDetailPageGroup` composable
- **New:** Single page pattern
  - `groups/[id].vue` - Combined layout and content in one file
  - No nested routing

### Auth Info Page
- **v0:** `/user/auth-info.vue`
- **New:** `/auth/info.vue`

## 10. Styling & Theme

### Color Mode
- **v0:** Has `@nuxtjs/color-mode` module with:
  - `ColorModeButtonHeadless.vue` component for toggling
  - Manual dark mode implementation throughout components
  - `.dark-mode` class selector
- **New:** No color mode toggle functionality (removed)

### Style File Organization
- **v0:** `app/assets/styles/utilities.scss`
- **New:** `app/styles/_utilities.scss`

### Theme Customization
- **v0:** Uses default Lara theme from PrimeVue
- **New:** Extensive PrimeVue theme customization with:
  - Emerald primary color palette
  - Custom surface colors for light mode (neutral)
  - Custom surface colors for dark mode (custom gray scale)
  - definePreset usage

## 11. Data Fetching Pattern

### v0 Pattern
Uses direct `useLazyBackendApi` calls with manual error/loading handling:
```typescript
const query: Query<GetGroupsResponse> = await useLazyBackendApi('/Groups', { key: 'groups' });
const isLoading = computed(() => query.status.value === 'pending');
```

### New Pattern
Uses TanStack Query with dedicated composables:
- `useGetGroupsQuery.ts` - Query for groups list
- `useGetGroupQuery.ts` - Query for single group
- `useCreateGroupMutation.ts` - Mutation for creating groups
- `useCreateExpenseMutation.ts` - Mutation for creating expenses
- `useDetailPageGroup.ts` - Shared group state for detail pages
- `queryKeys.ts` - Centralized query key management

**Location in new:** `app/composables/backend-api/`

**Benefits of new pattern:**
- Better caching and invalidation
- Automatic refetching
- Optimistic updates
- Better TypeScript inference

**Lost in migration:**
- Direct useLazyBackendApi calls are more verbose
- Need to create composables for each endpoint

## 12. Configuration Differences

### nuxt.config.ts
- **Module order:** Different between versions
- **Security module:** New version has `nuxt-security` in modules array, v0 has empty config
- **OpenFetch config:** Both have it but in different order in file
- **Runtime config structure:** Slightly different Redis configuration structure

## 13. Documentation

- **v0:** Has detailed `frontend-v0/CLAUDE.md` with:
  - Frontend-specific development commands
  - Architecture & patterns guide
  - Component structure templates
  - Data fetching patterns
  - Form handling examples
  - Authentication flow documentation
  - State management patterns
  - BFF pattern documentation
  - SCSS conventions
  - Common pitfalls section
  - Adding new features checklist
  - OpenTelemetry integration documentation
- **New:** No frontend-specific CLAUDE.md (relies on root-level CLAUDE.md)

## Summary Statistics

- **v0:** ~38 component/composable files
- **New:** ~23 component/composable files
- **Reduction:** ~40% fewer files in the new version

## Overall Assessment

The new version represents a modernization effort focusing on:
- TanStack Query for data management (better caching, invalidation)
- Regle for form validation (replacing Valibot with PrimeVue Forms)
- Simplified architecture with fewer files

However, significant UI/UX features are missing:
- **Critical:** Error handling components (ApiError)
- **Critical:** Loading states (LoadingIndicator, PageLoadingIndicator, Skeletons)
- **Critical:** Delete group functionality
- **Important:** User menu and authentication UI
- **Important:** Dark mode support
- **Important:** Back button navigation component
- **Nice-to-have:** Member avatars
- **Nice-to-have:** Form field labels with required indicators
- **Nice-to-have:** Reusable layout components (AppShell, AppContainer, AppCard)

## Next Steps

When bringing features from v0 to the new version, consider:
1. **Start with critical UI components:** ApiError, LoadingIndicator, PageLoadingIndicator
2. **Add delete functionality:** Adapt to new TanStack Query pattern
3. **Restore user menu:** AppHeaderUserInfo and related components
4. **Consider dark mode:** Decide if color mode should be restored
5. **Adapt patterns:** Update v0 components to use new data fetching patterns where applicable
6. **Update documentation:** Create frontend-specific CLAUDE.md if needed
