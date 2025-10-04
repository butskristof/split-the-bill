import type { H3Event } from 'h3';
import type { PersistentSession } from 'nuxt-oidc-auth/runtime/types.js';
import { getUserSessionId } from 'nuxt-oidc-auth/runtime/server/utils/session.js';
import { decryptToken } from 'nuxt-oidc-auth/runtime/server/utils/security.js';

export const getAccessToken = async (event: H3Event): Promise<string | null> => {
  const sessionId = await getUserSessionId(event);

  const persistentSession = (await useStorage('oidc').getItem<PersistentSession>(
    sessionId,
  )) as PersistentSession | null;
  if (persistentSession == null) return null;

  const tokenKey = process.env.NUXT_OIDC_TOKEN_KEY as string;
  return await decryptToken(persistentSession.accessToken, tokenKey);
};
