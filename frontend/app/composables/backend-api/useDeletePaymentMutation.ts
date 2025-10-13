import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';

export function useDeletePaymentMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ groupId, paymentId }: { groupId: string; paymentId: string }) =>
      $backendApi('/Groups/{groupId}/payments/{paymentId}', {
        method: 'DELETE',
        path: {
          groupId,
          paymentId,
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
