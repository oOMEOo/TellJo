using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ReadLater5.Auth
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private ReadLaterDataContext _readLaterDataContext;
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ReadLaterDataContext readLaterDataContext) : base(options, logger, encoder, clock)
        {
            _readLaterDataContext = readLaterDataContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderName, out var apiKey) || apiKey.Count != 1)
            {
                Logger.LogWarning("An API request was received without the x-api-key header");
                return AuthenticateResult.Fail("Invalid parameters");
            }

            var userApiKey = _readLaterDataContext.UserApiKey.Where(u => u.ApiKey.ToLower() == apiKey.ToString().ToLower()).FirstOrDefault();

            if (userApiKey == null)
            {
                Logger.LogWarning($"An API request was received with an invalid API key: {apiKey}");
                return AuthenticateResult.Fail("Invalid parameters");
            }

            Logger.BeginScope("{ClientId}", userApiKey.UserId);
            Logger.LogInformation("Client authenticated");

            var claims = new[] { new Claim(ClaimTypes.Name, userApiKey.UserId.ToString()) };
            var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
