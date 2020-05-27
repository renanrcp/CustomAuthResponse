using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CustomAuthResponse
{
    public static class CustomAuthorizationExtensions
    {
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
            => app.UseMiddleware<CustomAuthorizationMiddleware>();

        public static IServiceCollection UseAuthorizationResponseFilter<T>(this IServiceCollection services)
            where T : class, ICustomAuthorizationFilter
            => services.AddTransient<ICustomAuthorizationFilter, T>();
    }
}