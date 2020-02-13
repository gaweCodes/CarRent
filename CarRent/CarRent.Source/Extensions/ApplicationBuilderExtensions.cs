using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace CarRent.Source.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSpaRouting(this IApplicationBuilder builder)
        {
            return builder
                .Use((context, next) =>
                {
                    context.Request.Path = "/index.html";
                    return next.Invoke();
                })
                .UseStaticFiles();
        }
    }
}
