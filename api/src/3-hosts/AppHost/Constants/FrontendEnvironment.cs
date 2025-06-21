namespace SplitTheBill.AppHost.Constants;

internal static class FrontendEnvironment
{
    internal const string BackendBaseUrl = "frontend-backend-base-url";
    internal const string OidcRedirectUri = "frontend-oidc-redirect-uri";

    internal const string OidcClientId = "frontend-oidc-client-id";
    internal const string OidcClientSecret = "frontend-oidc-client-secret";
    internal const string OidcOpenIdConfiguration = "frontend-oidc-openid-config";
    internal const string OidcAuthorizationUrl = "frontend-oidc-authorization-url";
    internal const string OidcTokenUrl = "frontend-oidc-token-url";
    internal const string OidcUserInfoUrl = "frontend-oidc-userinfo-url";
    internal const string OidcLogoutUrl = "frontend-oidc-logout-url";
    internal const string OidcTokenKey = "frontend-oidc-token-key";
    internal const string OidcSessionSecret = "frontend-oidc-session-secret";
    internal const string OidcAuthSessionSecret = "frontend-oidc-auth-session-secret";

    internal static IDictionary<string, string> EnvironmentVariables =>
        new Dictionary<string, string>
        {
            { BackendBaseUrl, "NUXT_BACKEND_BASE_URL" },
            { OidcRedirectUri, "NUXT_OIDC_PROVIDERS_OIDC_REDIRECT_URI" },
            { OidcClientId, "NUXT_OIDC_PROVIDERS_OIDC_CLIENT_ID" },
            { OidcClientSecret, "NUXT_OIDC_PROVIDERS_OIDC_CLIENT_SECRET" },
            { OidcOpenIdConfiguration, "NUXT_OIDC_PROVIDERS_OIDC_OPEN_ID_CONFIGURATION" },
            { OidcAuthorizationUrl, "NUXT_OIDC_PROVIDERS_OIDC_AUTHORIZATION_URL" },
            { OidcTokenUrl, "NUXT_OIDC_PROVIDERS_OIDC_TOKEN_URL" },
            { OidcUserInfoUrl, "NUXT_OIDC_PROVIDERS_OIDC_USER_INFO_URL" },
            { OidcLogoutUrl, "NUXT_OIDC_PROVIDERS_OIDC_LOGOUT_URL" },
            { OidcTokenKey, "NUXT_OIDC_TOKEN_KEY" },
            { OidcSessionSecret, "NUXT_OIDC_SESSION_SECRET" },
            { OidcAuthSessionSecret, "NUXT_OIDC_AUTH_SESSION_SECRET" }
        };
}
