using IDP.Models.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IDP.Services;
using IdentityServer4.Validation;
using System.Security.Cryptography.X509Certificates;
using IDP.Const;
using IDP.Extentions;


JObject Settings = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("./appsettings.json"));

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(Settings["Settings"]["BaseURL"].ToString());
builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1073741824;
    options.Limits.MaxConcurrentConnections = 5000;
});

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

#region Default Thread Settings for parallel processing
int minWorker, minIOC;
ThreadPool.GetMinThreads(out minWorker, out minIOC);
ThreadPool.SetMinThreads(3000, minIOC);
#endregion


builder.Services.AddIdentityServer(options =>
{
    options.InputLengthRestrictions.Password = int.MaxValue;
    options.InputLengthRestrictions.UserName = int.MaxValue;
    options.InputLengthRestrictions.ClientSecret = int.MaxValue;
    options.IssuerUri = Settings["Settings"]["IssuerURL"].ToString();
})
    .AddInMemoryApiResources(Config.GetApis())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryApiScopes(Config.GetApisScopes())
    .AddDeveloperSigningCredential()
    .AddProfileService<CustomProfileService>();

builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddCertificateForwarding(options =>
{
    options.CertificateHeader = Config.SSL_CUSTOME_HEADER;
    options.HeaderConverter = (headerValue) =>
    {
        X509Certificate2? clientCertificate = null;

        if (!string.IsNullOrWhiteSpace(headerValue))
        {
            byte[] bytes = Convert.FromBase64String(headerValue);
            clientCertificate = new X509Certificate2(bytes);
        }

        return clientCertificate;
    };
});
builder.Services.AddAntiforgery();

var app = builder.Build();

app.UseStaticFiles();
app.UseCertificateForwarding();
app.UseEnableRequestRewindMiddleware();
app.UseExceptionMiddleware();
app.UseIdentityServer();

app.Run();