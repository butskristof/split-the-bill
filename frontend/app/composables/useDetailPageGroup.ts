import type { Group, GroupMember } from '~/types';

// https://vuejs.org/guide/typescript/composition-api.html#typing-provide-inject
// set up the key as a typed InjectionKey to sync the type between provide and inject
const key = Symbol() as InjectionKey<Ref<Group | null>>;

// this composable is intended to be used within the detail routes for groups
// (e.g. /groups/[id], /groups/[id]/members) to provide access to the data fetched at the parent
// component for the nested child routes
// it'll also contain utilities for extracting or manipulating the data in this group, however this
// should be considered for refactoring into its own utility or composable if too many get added
// (separation of concern)
export const useDetailPageGroup = () => {
  // function to provide a ref to use in child routes, use this is in the topmost component which
  // is fetching the data
  // make note that we're provide/inject-ing a ref here, so you should only call this if the actual
  // ref changes, not its value
  const provideGroup = (groupRef: Ref<Group | null>): void => {
    provide(key, groupRef);
  };
  // inject the provided ref which will return a group or null, also falls back to a ref(null) in
  // case the app isn't initialised yet (this can get ugly: no reactivity in case a new value is
  // provided!)
  const injectGroup = (): Ref<Group | null> => inject<Ref<Group | null>>(key, ref(null));
  // actually inject the ref here to use it for the utilities below, or expose it out of the
  // composable
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
