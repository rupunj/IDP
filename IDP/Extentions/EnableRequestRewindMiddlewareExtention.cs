using IDP.Handlers;

namespace IDP.Extentions
{
    public static class EnableRequestRewindMiddlewareExtention
    {
        public static void UseEnableRequestRewindMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<EnableRequestRewindMiddleware>();
        }
    }
}
