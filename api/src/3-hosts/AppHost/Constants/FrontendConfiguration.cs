namespace SplitTheBill.AppHost.Constants;

internal static class FrontendConfiguration
{
    internal static readonly AspireParameter BackendBaseUrl = new(
        "frontend-backend-base-url",
        "NUXT_BACKEND_BASE_URL"
    );
    internal static readonly AspireParameter OidcRedirectUri = new(
        "frontend-oidc-redirect-uri",
        "NUXT_OIDC_PROVIDERS_OIDC_REDIRECT_URI"
    );

    internal static readonly AspireParameter OidcClientId = new(
        "frontend-oidc-client-id",
        "NUXT_OIDC_PROVIDERS_OIDC_CLIENT_ID"
    );
    internal static readonly AspireParameter OidcClientSecret = new(
        "frontend-oidc-client-secret",
        "NUXT_OIDC_PROVIDERS_OIDC_CLIENT_SECRET"
    );
    internal static readonly AspireParameter OidcOpenIdConfiguration = new(
        "frontend-oidc-openid-config",
        "NUXT_OIDC_PROVIDERS_OIDC_OPEN_ID_CONFIGURATION"
    );
    internal static readonly AspireParameter OidcAuthorizationUrl = new(
        "frontend-oidc-authorization-url",
        "NUXT_OIDC_PROVIDERS_OIDC_AUTHORIZATION_URL"
    );
    internal static readonly AspireParameter OidcTokenUrl = new(
        "frontend-oidc-token-url",
        "NUXT_OIDC_PROVIDERS_OIDC_TOKEN_URL"
    );
    internal static readonly AspireParameter OidcUserInfoUrl = new(
        "frontend-oidc-userinfo-url",
        "NUXT_OIDC_PROVIDERS_OIDC_USER_INFO_URL"
    );
    internal static readonly AspireParameter OidcLogoutUrl = new(
        "frontend-oidc-logout-url",
        "NUXT_OIDC_PROVIDERS_OIDC_LOGOUT_URL"
    );
    internal static readonly AspireParameter OidcTokenKey = new(
        "frontend-oidc-token-key",
        "NUXT_OIDC_TOKEN_KEY"
    );
    internal static readonly AspireParameter OidcSessionSecret = new(
        "frontend-oidc-session-secret",
        "NUXT_OIDC_SESSION_SECRET"
    );
    internal static readonly AspireParameter OidcAuthSessionSecret = new(
        "frontend-oidc-auth-session-secret",
        "NUXT_OIDC_AUTH_SESSION_SECRET"
    );

    internal static readonly AspireParameter RedisHost = new(
        "frontend-redis-host",
        "NUXT_REDIS_HOST"
    );
    internal static readonly AspireParameter RedisPort = new(
        "frontend-redis-port",
        "NUXT_REDIS_PORT"
    );
    internal static readonly AspireParameter RedisPassword = new(
        "frontend-redis-password",
        "NUXT_REDIS_PASSWORD"
    );
}
