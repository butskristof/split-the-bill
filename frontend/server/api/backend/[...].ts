import { getUserSession } from 'nuxt-oidc-auth/runtime/server/utils/session.js';
import { getAccessToken } from '~~/server/utils/auth';
import { joinURL } from 'ufo';

// This catch-all endpoint proxies all incoming requests to the "backend" remote API
export default defineEventHandler(async (event) => {
  if (!event.headers.get('x-csrf'))
    throw createError({ statusCode: 401, statusMessage: 'Missing CSRF protection header' });

  // get the full path from the request and strip out the `/api/backend` prefix
  const path = event.path.replace(/^\/api\/backend\//, '');

  // handle authentication
  // since nuxt-oidc-auth in its current version cannot return the access tokens in the UserSession
  // object without exposing them to the client as well, we hack our way around this:
  // 1. use the getUserSession function as described in the docs: this takes care of expired
  //    sessions, refreshing, ...
  //    it throws if it cannot provide a UserSession object, we catch and rethrow with just
  //    401 Unauthorized instead of the detailed error we get from the library
  // 2. use a utility function to extract the access token from the persistent session storage
  //    we already know there is a user session at this point, and since we're using refresh tokens,
  //    the access token will have been saved to the storage
  //    the code itself is lifted from the library, where it is behind a configuration check for
  //    exposeAccessToken but no differentiation for calls from the client client (composable in o
  //    the app) or server. We copied the code and keep it in here so it'll only ever run on the
  //    server
  //    !! MAKE SURE TO NEVER EXPOSE THE ACCESS TOKEN IN THE RESPONSE !!

  // make sure session exists, will throw 401 error otherwise
  await getUserSession(event);
  const accessToken = await getAccessToken(event); // extract access token manually

  const baseUrl = useRuntimeConfig(event).backendBaseUrl;
  console.log(baseUrl);
  const target = joinURL(baseUrl, path);
  console.log(target);
  return proxyRequest(event, target, {
    headers: {
      authorization: `Bearer ${accessToken}`,
    },
  });
});
