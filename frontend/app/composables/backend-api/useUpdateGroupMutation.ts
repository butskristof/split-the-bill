import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';
import type { UpdateGroupRequest } from '#shared/types/api';

export function useUpdateGroupMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateGroupRequest): Promise<void> =>
      $backendApi('/Groups/{id}', {
        method: 'PUT',
        body: data,
        path: {
          id: data.id,
        },
      }) as Promise<void>,
    onSuccess: (_data, request: UpdateGroupRequest) => {
      // Invalidate groups list to trigger refetch
      queryClient.invalidateQueries({ queryKey: queryKeys.groups.all });
      // Invalidate the specific group's detail query to trigger refetch
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(request.id),
      });
    },
  });
}
