import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { queryKeys } from './queryKeys';
import type { UpdateExpenseRequest } from '#shared/types/api';

export function useUpdateExpenseMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateExpenseRequest): Promise<void> =>
      $backendApi('/Groups/{groupId}/expenses/{expenseId}', {
        method: 'PUT',
        body: data,
        path: {
          groupId: data.groupId,
          expenseId: data.id,
        },
      }) as Promise<void>,
    onSuccess: (_data, request: UpdateExpenseRequest) => {
      // Invalidate the specific group's detail query to trigger refetch
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(request.groupId),
      });
    },
  });
}
