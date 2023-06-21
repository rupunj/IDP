using IDP.Handlers;

namespace IDP.Extentions
{
    public static class ExceptionMiddlewareExtention
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
