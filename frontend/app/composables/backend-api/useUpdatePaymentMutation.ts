import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';
import type { UpdatePaymentRequest } from '#shared/types/api';

export function useUpdatePaymentMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdatePaymentRequest): Promise<void> =>
      $backendApi('/Groups/{groupId}/payments/{paymentId}', {
        method: 'PUT',
        body: data,
        path: {
          groupId: data.groupId,
          paymentId: data.id,
        },
      }) as Promise<void>,
    onSuccess: (_data, request: UpdatePaymentRequest) => {
      // Invalidate the specific group's detail query to trigger refetch
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(request.groupId),
      });
    },
  });
}
