import { breakpointsTailwind } from '@vueuse/core';

const useAppBreakpoints = () => {
  // currently, the breakpoints in _variables.scss are aligned to the Tailwind values, so we
  // can use the preset here
  // make sure to keep those in sync should other breakpoints be used
  const vueUseBreakpoints = useBreakpoints(breakpointsTailwind);
  return vueUseBreakpoints;
};

export default useAppBreakpoints;
