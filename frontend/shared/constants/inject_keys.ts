import type { Group } from '#shared/api-clients/split-the-bill-api/types';

const GROUP = Symbol() as InjectionKey<ComputedRef<Group | undefined>>;

export const INJECTION_KEYS = {
  GROUP,
};
