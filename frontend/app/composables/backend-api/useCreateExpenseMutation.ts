import { useMutation, useQueryClient } from '@tanstack/vue-query';
import type { CreateExpenseRequest } from '#shared/types/api';
import { queryKeys } from './queryKeys';

export function useCreateExpenseMutation() {
  const { $backendApi } = useNuxtApp();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (newExpense: CreateExpenseRequest) =>
      $backendApi('/Groups/{groupId}/expenses', {
        method: 'POST',
        path: {
          groupId: newExpense.groupId,
        },
        body: newExpense,
      }),
    onSuccess: (_data, request) => {
      // Invalidate the specific group's detail query to trigger refetch
      queryClient.invalidateQueries({
        queryKey: queryKeys.groups.detail(request.groupId),
      });
    },
  });
}
