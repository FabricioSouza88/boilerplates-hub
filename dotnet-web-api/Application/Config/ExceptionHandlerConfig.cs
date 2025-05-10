using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Config
{
    public static class ExceptionHandlerConfig
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature != null)
                    {
                        var ex = exceptionHandlerPathFeature.Error;

                        logger.LogError(ex, "Unexpected error: {Message}", ex.Message);

                        var response = new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal server error. Try again latter."
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });
        }
    }
}
