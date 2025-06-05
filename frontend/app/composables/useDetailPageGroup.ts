import type { Group, GroupMember } from '~/types';

const key = Symbol() as InjectionKey<Ref<Group | null>>;

export const useDetailPageGroup = () => {
  const provideGroup = (groupRef: Ref<Group | null>): void => {
    provide(key, groupRef);
  };
  const injectGroup = (): Ref<Group | null> => inject<Ref<Group | null>>(key, ref(null));
  const group = injectGroup();

  const members = computed<GroupMember[]>(() => group?.value?.members ?? []);
  const getMember = (id: string): GroupMember | undefined =>
    members.value?.find((m) => m.id === id);

  return {
    provideGroup,
    injectGroup,
    group,
    members,
    getMember,
  };
};
