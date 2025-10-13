import { useGetGroupQuery } from './useGetGroupQuery';
import type {
  Activity,
  Expense,
  ExpenseActivity,
  Payment,
  PaymentActivity,
} from '#shared/types/api';

/**
 * Route-aware composable for fetching the current group in detail pages.
 * Automatically extracts the group ID from the route params.
 */
export function useDetailPageGroup() {
  const route = useRoute();
  const groupId = computed(() => route.params.id as string);
  const query = useGetGroupQuery(groupId);
  const group = computed<Group | null>(() => query.data.value ?? null);
  const members = computed(() => group.value?.members ?? null);
  const expenses = computed(() => group.value?.expenses ?? null);
  const payments = computed(() => group.value?.payments ?? null);

  const activities = computed<Activity[] | null>(() =>
    group != null ? getActivities(group.value as Group) : null,
  );

  return {
    ...query,
    groupId,
    group,
    members,
    expenses,
    payments,
    activities,
  };
}

export const getActivities = (group: Group): Activity[] => {
  const activity: Activity[] = [];

  // Cast from loose OpenAPI types to strict DTOs at the boundary
  const e = (group.expenses ?? []) as Expense[];
  const p = (group.payments ?? []) as Payment[];

  activity.push(...e.map((e): ExpenseActivity => ({ ...e, type: 'expense' })));
  activity.push(...p.map((p): PaymentActivity => ({ ...p, type: 'payment' })));

  activity.sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime());

  return activity;
};
