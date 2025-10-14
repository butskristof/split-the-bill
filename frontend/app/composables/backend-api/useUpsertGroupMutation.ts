import { useMutation, useQueryClient } from '@tanstack/vue-query';
import type { components } from '#open-fetch-schemas/backend-api';
import { queryKeys } from './queryKeys';
import type { CreateGroupRequest, UpdateGroupRequest } from '#shared/types/api';

type CreateGroupResponse = components['schemas']['CreateGroup.Response'];

export function useUpsertGroupMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (
      data: CreateGroupRequest | UpdateGroupRequest,
    ): Promise<CreateGroupResponse | undefined> =>
      'id' in data
        ? $backendApi('/Groups/{id}', {
            method: 'PUT',
            body: data,
            path: {
              id: data.id,
            },
          })
        : ($backendApi('/Groups', {
            method: 'POST',
            body: data,
          }) as Promise<CreateGroupResponse>),
    onSuccess: (_data, variables) => {
      // Invalidate groups list to trigger refetch
      queryClient.invalidateQueries({ queryKey: queryKeys.groups.all });

      // If updating, also invalidate the specific group detail query
      if ('id' in variables) {
        queryClient.invalidateQueries({ queryKey: queryKeys.groups.detail(variables.id) });
      }
    },
  });
}
