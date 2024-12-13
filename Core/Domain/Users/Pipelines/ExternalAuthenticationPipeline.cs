using SmaHauJenHoaVij.Core.Domain.Users.Handlers;
public static class ExternalAuthenticationPipelineExtensions
{
    public static IApplicationBuilder UseExternalAuthenticationPipeline(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var path = context.Request.Path.Value;

            if (path.Equals("/ExternalLogin", StringComparison.OrdinalIgnoreCase))
            {
                var provider = context.Request.Query["provider"];
                var returnUrl = context.Request.Query["returnUrl"];

                var handler = context.RequestServices.GetRequiredService<ExternalLoginHandler>();
                await handler.HandleAsync(context, provider, returnUrl);
            }
            else if (path.Equals("/ExternalLoginCallback", StringComparison.OrdinalIgnoreCase))
            {
                var returnUrl = context.Request.Query["returnUrl"];

                var handler = context.RequestServices.GetRequiredService<ExternalLoginCallbackHandler>();
                await handler.HandleAsync(context, returnUrl);
            }
            else
            {
                await next(); // Pass to next middleware if no match
            }
        });
    }
}
