import { getUserSession } from 'nuxt-oidc-auth/runtime/server/utils/session.js';
import { getAccessToken } from '~~/server/utils/auth';
import { joinURL } from 'ufo';
import { stringIsNullOrWhitespace } from '~~/shared/utils';

// This catch-all endpoint proxies all incoming requests to the "backend" remote API
export default defineEventHandler(async (event) => {
  // no use in continuing if the upstream base URL is missing
  const baseUrl = useRuntimeConfig(event).backendBaseUrl;
  if (stringIsNullOrWhitespace(baseUrl))
    throw createError({ statusCode: 500, statusMessage: 'Failed to retrieve backend base URL' });

  // custom header must be set to protect against CSRF attacks
  // https://docs.duendesoftware.com/bff/#csrf-attacks
  // setting a custom header triggers a CORS preflight check on EVERY request (even "simple" ones
  // such as a GET without additional configuration), this way we effectively force "same site" to be
  // "same (or allowed) origin"
  if (event.headers.get('x-csrf') !== '1')
    throw createError({ statusCode: 400, statusMessage: 'Missing CSRF protection header' });

  // /api/backend/[whatever] -> extract [whatever]
  const remotePath = event.path.replace(/\/api\/backend/, '');
  if (stringIsNullOrWhitespace(remotePath))
    throw createError({ statusCode: 500, statusMessage: 'Failed to determine remote path' });

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
  if (stringIsNullOrWhitespace(accessToken))
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });

  const target = joinURL(baseUrl, remotePath!);
  return proxyRequest(event, target, {
    headers: {
      // all headers from original request are also passed along
      authorization: `Bearer ${accessToken!}`, // set auth header
      cookie: '', // do not forward (auth) cookies to remote API
    },
    fetchOptions: {
      // do not follow redirect (such as to login pages), but pass back the 3XX response
      redirect: 'manual',
    },
  });
});
