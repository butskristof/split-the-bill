// https://nitro.build/guide/storage#runtime-configuration
// https://github.com/nitrojs/nitro/issues/1161#issuecomment-1511444675
// using storage with dynamic configuration isn't really support yet, but can't rely on static
// config in nuxt.config.ts or localhost:6379 existing since we'll need to use the Redis instance
// provided by Aspire or Docker
// this plugins picks up the oidc storage config initiated in nuxt.config.ts and adds the connection
// parameters from runtime config
// might need to apply this to other mounts as well later on?
export default defineNitroPlugin(async () => {
  const storage = useStorage();

  const runtimeConfig = useRuntimeConfig();

  const mount = storage.getMount('oidc');
  const driver = mount.driver;
  driver.options.host = runtimeConfig.redis.host;
  driver.options.port = runtimeConfig.redis.port;
  driver.options.password = runtimeConfig.redis.password;
});
