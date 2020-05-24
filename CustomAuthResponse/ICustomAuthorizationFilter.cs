using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthResponse
{
    public interface ICustomAuthorizationFilter
    {
        Task HandleAsync(RequestDelegate next, ActionContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult);
    }
}