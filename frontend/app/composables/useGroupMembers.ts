import type { Group, GroupMember } from '~/components/groups/detail/types';

const useGroupMembers = () => {
  const group = inject<ComputedRef<Group>>('group');
  const members = computed<GroupMember[]>(() => group?.value?.members ?? []);

  const getMember = (id: string): GroupMember | undefined =>
    members.value?.find((m) => m.id === id);

  return {
    members,
    getMember,
  };
};

export default useGroupMembers;
