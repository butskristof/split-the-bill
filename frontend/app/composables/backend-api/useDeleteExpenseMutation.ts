import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';

export function useDeleteExpenseMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ groupId, expenseId }: { groupId: string; expenseId: string }) =>
      $backendApi('/Groups/{groupId}/expenses/{expenseId}', {
        method: 'DELETE',
        path: {
          groupId,
          expenseId,
        },
      }),
    onSuccess: (_data, { groupId }) => {
      // Invalidate the specific group's detail query to refresh the group data
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(groupId),
      });
    },
  });
}
