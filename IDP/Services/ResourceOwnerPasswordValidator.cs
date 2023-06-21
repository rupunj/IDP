using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using IDP.Models.Base;
using IDP.Enums;

namespace IDP.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private Settings Config { get; set; }

        private readonly HttpContext context;

        public ResourceOwnerPasswordValidator(IHttpContextAccessor _httpContextAccessor, IOptions<Settings> _settings)
        {
            context = _httpContextAccessor.HttpContext;
            Config = _settings.Value;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if(context.UserName.Equals("Admin", StringComparison.InvariantCultureIgnoreCase) && context.Password.Equals("abc123++"))
                context.Result = new GrantValidationResult(subject: "Admin", authenticationMethod: "pwd", identityProvider: "Granter IDP");
            else if (context.UserName.Equals("User", StringComparison.InvariantCultureIgnoreCase) && context.Password.Equals("abc123++"))
                context.Result = new GrantValidationResult(subject: "User", authenticationMethod: "pwd", identityProvider: "Granter IDP");
            else if (context.UserName.Equals("Auditor", StringComparison.InvariantCultureIgnoreCase) && context.Password.Equals("abc123++"))
                context.Result = new GrantValidationResult(subject: "Auditor", authenticationMethod: "pwd", identityProvider: "Granter IDP");
            else
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, customResponse: new Dictionary<string, object>() { { "Application", context.Request.Client.ClientId }, { "Reason", AuthReason.InvalidUsernameOrPassword } });

            return;
        }
    }
}
