import { useGetGroupQuery } from './useGetGroupQuery';

/**
 * Route-aware composable for fetching the current group in detail pages.
 * Automatically extracts the group ID from the route params.
 */
export function useDetailPageGroup() {
  const route = useRoute();
  const groupId = computed(() => route.params.id as string);
  return useGetGroupQuery(groupId);
}
