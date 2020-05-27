using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthResponse.Sample.Middlewares
{
    public class CustomAuthorizationHandler : ICustomAuthorizationFilter
    {
        public async Task HandleAsync(RequestDelegate next, ActionContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Succeeded)
            {
                await next(context.HttpContext);
            }
            else
            {
                var result = new ObjectResult("You must be logged in.")
                {
                    StatusCode = 401
                };

                await result.ExecuteResultAsync(context);
            }
        }
    }
}