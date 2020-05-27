using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthResponse
{
    internal class DefaultCustomAuthorizationFilter : ICustomAuthorizationFilter
    {
        public async Task HandleAsync(RequestDelegate next, ActionContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {
                if (policy.AuthenticationSchemes.Count > 0)
                {
                    foreach (var scheme in policy.AuthenticationSchemes)
                    {
                        await context.HttpContext.ChallengeAsync(scheme);
                    }
                }
                else
                {
                    await context.HttpContext.ChallengeAsync();
                }

                return;
            }
            else if (authorizeResult.Forbidden)
            {
                if (policy.AuthenticationSchemes.Count > 0)
                {
                    foreach (var scheme in policy.AuthenticationSchemes)
                    {
                        await context.HttpContext.ForbidAsync(scheme);
                    }
                }
                else
                {
                    await context.HttpContext.ForbidAsync();
                }

                return;
            }

            await next(context.HttpContext);
        }
    }
}