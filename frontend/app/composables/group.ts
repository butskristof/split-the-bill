import { INJECTION_KEYS } from '#shared/constants/inject_keys';
import type { Group } from '#shared/api-clients/split-the-bill-api/types';

export const useCurrentPageGroupId = () => {
  const route = useRoute();
  return route.params.id as string;
};

export const useGroup = async (id?: string) => {
  const groupId = id || useCurrentPageGroupId();

  const { getGroup } = useSplitTheBillApi();
  const { data: group, isError, isPending, error } = await getGroup(groupId);

  return {
    isPending,
    isError,
    error,
    group,
  };
};

export const useProvideGroup = (group: ComputedRef<Group | undefined>) => {
  provide(INJECTION_KEYS.GROUP, group);
};

export const useInjectGroup = () => {
  return inject(INJECTION_KEYS.GROUP);
};
