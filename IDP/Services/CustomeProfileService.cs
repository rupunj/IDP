using IdentityServer4.Models;
using IdentityServer4.Services;
using IDP.Const;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using IDP.Models.Base;

namespace IDP.Services
{
    public class CustomProfileService : IProfileService
    {
        private Settings Config { get; set; }
        private readonly HttpContext context;

        public CustomProfileService(IHttpContextAccessor _httpContextAccessor, IOptions<Settings> _settings)
        {
            context = _httpContextAccessor.HttpContext;
            Config = _settings.Value;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims = new List<Claim>();

            string requestedRoles = "";

            if (context.ValidatedRequest.Raw["username"].ToString().Equals("Admin", StringComparison.InvariantCultureIgnoreCase))
                requestedRoles = "Admin";
            else if (context.ValidatedRequest.Raw["username"].ToString().Equals("User", StringComparison.InvariantCultureIgnoreCase))
                requestedRoles = "User";
            else if (context.ValidatedRequest.Raw["username"].ToString().Equals("Auditor", StringComparison.InvariantCultureIgnoreCase))
                requestedRoles = "Auditor";

            if (requestedRoles.Length > 0)
                context.IssuedClaims.Add(new Claim("role", requestedRoles));

            if (context.IssuedClaims.Count > 0)
                return Task.CompletedTask;

            return Task.FromException(new Exception(ErrorStatus.STATUS_MSG_INSUFFICIENT_PRIVILEGES));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(true);
        }
    }
}
