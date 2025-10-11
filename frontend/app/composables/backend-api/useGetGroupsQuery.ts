import { useQuery } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';

export function useGetGroupsQuery() {
  const { $backendApi } = useNuxtApp();

  return useQuery({
    queryKey: queryKeys.groups.all,
    queryFn: () => $backendApi('/Groups'),
    select: (data) => data.groups ?? [],
  });
}
