using IdentityModel;
using IdentityServer4.Models;

namespace IDP.Const
{
    public class Config
    {
        internal const string CONTENT_TYPE_APPLICATION_JSON = "application/json";
        public const string TRACE_CUSTOME_HEADER = "X-Correlation-ID";
        public const string SSL_CUSTOME_HEADER = "X-SSL-Cert";

        internal const int HTTP_STATUS_CODE_OK = 200;
        internal const int HTTP_STATUS_CODE_BAD = 400;

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("cart", "Cart Web"){ UserClaims = { JwtClaimTypes.Audience }, Scopes = new List<string> { "Create", "View", "Edit" } },
                new ApiResource("idp.api", "Identity API"){ UserClaims = { JwtClaimTypes.Audience }, Scopes = new List<string> { "Create", "View", "Edit" } },
                new ApiResource("cart.api","Cart API"){ UserClaims = { JwtClaimTypes.Audience }, Scopes = new List<string> { "Create", "View", "Edit" } },
                new ApiResource("stock.api","Stock API"){ UserClaims = { JwtClaimTypes.Audience }, Scopes = new List<string> { "Create", "View", "Edit" } },
            };
        }

        public static IEnumerable<ApiScope> GetApisScopes()
        {
            return new ApiScope[]
            {
                new ApiScope("Create", "Create Item"),
                new ApiScope("View","View Item"),
                new ApiScope("Edit","Edit Item")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "23238E32-0D83-11EE-BE56-0242AC120002",
                    ClientName = "Shopping Cart",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets = { new Secret("RjAwNjA1RTEtRURCQy00QkY1LTlDRTgtOTY5Mzg1RjE0MTc1".Sha256()) },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "idp.api", "cart.api", "stock.api", "Create", "View", "Edit" },
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute
                }
            };
        }
    }

    
}
