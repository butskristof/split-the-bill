import type { ApiResponse } from '~~/shared/api-clients/split-the-bill-api/api-client';
import type {
  GetGroupsResponse,
  GetMembersResponse,
  ProblemDetails,
} from '#shared/api-clients/split-the-bill-api/types';
import { SplitTeBillApiClient } from '~~/shared/api-clients/split-the-bill-api/api-client';

const KEYS = {
  GET_MEMBERS: 'MEMBERS_GET_ALL',
  GET_GROUPS: 'GROUPS_GET_ALL',
  GET_GROUP: (id: string): string => `GROUP_GET_${id}`,
};

type SplitTheBillApiResponse<T> = {
  data: ComputedRef<T | undefined>;
  error?: Ref<string>;
  isError: Ref<boolean>;
  isPending: Ref<boolean>;
};

const useSplitTheBillApiAsyncData = <T>(
  key: string,
  fn: () => Promise<ApiResponse<T>>,
): SplitTheBillApiResponse<T> => {
  const asyncData = useLazyAsyncData<ApiResponse<T>>(key, fn, {
    transform: (r: ApiResponse<T>) => ({ data: r.data, error: r.error }),
  });
  const data = computed<T | undefined>(() => asyncData.data.value?.data);
  const error = computed<string | ProblemDetails | undefined>(
    () => asyncData.error.value ?? asyncData.data.value?.error,
  );
  const isError = computed<boolean>(() => !!error.value);
  const isPending = computed<boolean>(() => asyncData.status.value === 'pending');

  return {
    ...asyncData,
    data,
    error,
    isError,
    isPending,
  };
};

export default () => {
  const runtimeConfig = useRuntimeConfig();
  const apiClient = new SplitTeBillApiClient(
    runtimeConfig.public.splitTheBillApi.baseUrl,
    () => runtimeConfig.public.splitTheBillApi.accessToken,
  );

  const getMembers = () =>
    useSplitTheBillApiAsyncData<GetMembersResponse>(KEYS.GET_MEMBERS, apiClient.getMembers);

  const getGroups = () =>
    useSplitTheBillApiAsyncData<GetGroupsResponse>(KEYS.GET_GROUPS, apiClient.getGroups);

  return {
    getMembers,
    getGroups,
  };
};
