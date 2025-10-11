import { useMutation, useQueryClient } from '@tanstack/vue-query';
import type { CreatePaymentRequest } from '#shared/types/api';
import { queryKeys } from './queryKeys';

export function useCreatePaymentMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (newPayment: CreatePaymentRequest) =>
      $backendApi('/Groups/{groupId}/payments', {
        method: 'POST',
        path: {
          groupId: newPayment.groupId,
        },
        body: newPayment,
      }),
    onSuccess: (_data, request) => {
      // Invalidate the specific group's detail query to trigger refetch
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(request.groupId),
      });
    },
  });
}
