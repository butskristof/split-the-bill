export default defineNuxtPlugin((nuxtApp) => {
  // Add required CSRF header to all backendApi requests
  nuxtApp.hook('openFetch:onRequest:backendApi', (ctx) => {
    ctx.options.headers.set('x-csrf', '1');
  });
});
