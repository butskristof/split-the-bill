import { useMutation, useQueryClient } from '@tanstack/vue-query';
import type { components } from '#open-fetch-schemas/backend-api';
import { queryKeys } from './queryKeys';
import type { CreateGroupRequest } from '#shared/types/api';

type CreateGroupResponse = components['schemas']['CreateGroup.Response'];

export function useCreateGroupMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateGroupRequest) =>
      $backendApi('/Groups', {
        method: 'POST',
        body: data,
      }) as Promise<CreateGroupResponse>,
    onSuccess: () => {
      // Invalidate groups list to trigger refetch
      queryClient.invalidateQueries({ queryKey: queryKeys.groups.all });
    },
  });
}
