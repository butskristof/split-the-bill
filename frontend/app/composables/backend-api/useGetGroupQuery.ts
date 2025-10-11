import { useQuery } from '@tanstack/vue-query';
import type { MaybeRefOrGetter } from 'vue';
import { queryKeys } from './queryKeys';

export function useGetGroupQuery(groupId: MaybeRefOrGetter<string>) {
  const { $backendApi } = useNuxtApp();
  const id = computed(() => toValue(groupId));

  return useQuery({
    queryKey: computed(() => queryKeys.groups.detail(id.value)),
    queryFn: () =>
      $backendApi('/Groups/{id}', {
        path: { id: id.value },
      }),
    enabled: computed(() => !!id.value),
  });
}
