import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';

export function useDeleteGroupMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (groupId: string) =>
      $backendApi('/Groups/{id}', {
        method: 'DELETE',
        path: {
          id: groupId,
        },
      }),
    onSuccess: (_data, groupId) => {
      // Invalidate the specific group's detail query
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(groupId),
      });
      // Invalidate groups list to trigger refetch
      queryClient.invalidateQueries({ queryKey: queryKeys.groups.all });
    },
  });
}
