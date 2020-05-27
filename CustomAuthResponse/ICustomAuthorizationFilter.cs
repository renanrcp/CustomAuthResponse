using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthResponse
{
    /// <summary>
    /// An interface that allows you to modify auth responses.
    /// </summary>
    public interface ICustomAuthorizationFilter
    {
        /// <summary>
        /// A function that can process an auth result and set a response.
        /// </summary>
        /// <param name="next">A function that can process an HTTP request.</param>
        /// <param name="context">Context object for execution of action which has been selected as 
        /// part of an HTTP request.</param>
        /// <param name="policy">Represents a collection of authorization requirements and the 
        /// scheme or schemes they are evaluated against, all of which must succeed for authorization 
        /// to succeed.</param>
        /// <param name="authorizeResult">The result of the authentication.</param>
        /// <returns>A <see cref="Task" /> representing an asynchronous operation for process
        /// the authentication result.</returns>
        Task HandleAsync(RequestDelegate next, ActionContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult);
    }
}