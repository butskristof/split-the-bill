/**
 * Centralized query key management for TanStack Query.
 */
export const queryKeys = {
  groups: {
    all: ['groups'] as const,
    detail: (id: string) => ['groups', id] as const,
  },
  expenses: {
    all: ['expenses'] as const,
    byGroup: (groupId: string) => ['expenses', 'group', groupId] as const,
  },
} as const;
