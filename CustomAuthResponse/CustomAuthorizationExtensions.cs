using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CustomAuthResponse
{
    /// <summary>
    /// Some extensions for add the CustomAuthResponse.
    /// </summary>
    public static class CustomAuthorizationExtensions
    {
        /// <summary>
        /// Add the <see cref="CustomAuthorizationMiddleware" /> to the app middlewares.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" /> with the 
        /// <see cref="CustomAuthorizationMiddleware" /> in the application middlewares.</returns>
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
            => app.UseMiddleware<CustomAuthorizationMiddleware>();

        /// <summary>
        /// Add a custom auth response handler to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The current dependency container.</param>
        /// <typeparam name="T">The typeof your custom auth response handler.</typeparam>
        /// <returns>The <see cref="IServiceCollection" /> with your custom auth response
        /// handler added.</returns>
        public static IServiceCollection UseAuthorizationResponseFilter<T>(this IServiceCollection services)
            where T : class, ICustomAuthorizationFilter
            => services.AddTransient<ICustomAuthorizationFilter, T>();
    }
}