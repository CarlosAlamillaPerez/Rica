using bepensa_biz.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace bepensa_ss_api.Configuratioin
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string ApiKeySchemeName = "ApiKey";

        private const string ApiKeyHeaderName = "X-API-KEY";

        private readonly ApiSettings app;

        private string Token { get; }

        public ApiKeyAuthenticationHandler(IOptionsSnapshot<ApiSettings> app,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder
            ) : base (options, logger, encoder)
        {
            this.app = app.Value;

            Token = this.app.ApiKeyDefault;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValue))
                return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));

            if (apiKeyHeaderValue != Token)
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));

            var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyDefault") };

            var identity = new ClaimsIdentity(claims, ApiKeySchemeName);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, ApiKeySchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
