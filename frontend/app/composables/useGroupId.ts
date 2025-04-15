export default () => {
  const route = useRoute();
  return route.params.id as string;
};
