using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CustomAuthResponse
{
    public class CustomAuthorizationMiddleware
    {
        private const string AuthorizationMiddlewareInvokedWithEndpointKey = "__AuthorizationMiddlewareWithEndpointInvoked";
        private readonly object AuthorizationMiddlewareWithEndpointInvokedValue = new object();

        private readonly RequestDelegate _next;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        public CustomAuthorizationMiddleware(RequestDelegate next, IAuthorizationPolicyProvider policyProvider)
        {
            _next = next;
            _policyProvider = policyProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.HasNoContent())
                return;

            var endpoint = context?.Features?.Get<IEndpointFeature>().Endpoint;

            if (endpoint.HasContent())
                context.Items[AuthorizationMiddlewareInvokedWithEndpointKey] = AuthorizationMiddlewareWithEndpointInvokedValue;

            var authorizeData = endpoint?.Metadata?.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();


            var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData);

            if (policy.HasNoContent())
            {
                await _next(context);
                return;
            }

            var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();

            var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context);

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var authorizeResult = await policyEvaluator.AuthorizeAsync(policy, authenticateResult, context, endpoint);

            if (authorizeResult.Succeeded)
            {
                await _next(context);
                return;
            }

            var routeData = context.GetRouteData() ?? new RouteData();

            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());

            var handler = context.RequestServices.GetService<ICustomAuthorizationFilter>() ?? new DefaultCustomAuthorizationFilter();

            await handler.HandleAsync(_next, actionContext, policy, authorizeResult);

            return;
        }

    }
}