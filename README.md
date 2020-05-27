# CustomAuthResponse
Custom Authorization library response for ASP.NET Core 3.1

This lib allows you to change authorization responses from ASP.NET Core 3.1

# Warning
Don't use this for .NET 5, this already implemented in the PR [#21117](https://github.com/dotnet/aspnetcore/pull/21117)

# Installation
You can add this lib via nuget package manager.

Dotnet cli example:
```bash
dotnet add package CustomAuthResponse
```
# Implementation

Create your custom auth middleware like this, you'll need to implement the ICustomAuthorizationFilter interface:
```cs
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
            var result = new ObjectResult("You must be logged.")
            {
                StatusCode = 401
            };

            await result.ExecuteResultAsync(context);
        }
    }
}
```
Your Startup class will look something like this:
```cs
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Any authentication scheme or policy.
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

        services.AddAuthorization();

        // Add your custom handler.
        services.UseAuthorizationResponseFilter<CustomAuthorizationHandler>();

        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // if (env.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        // }

        // app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        //app.UseAuthorization();

        // Don't use the default ASP.NET Core authorization
        // middleware and add the CustomAuthorizationMiddleware.
        app.UseCustomAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

You can see a sample webapi with this lib [here](https://github.com/renanrcp/CustomAuthResponse/tree/master/CustomAuthResponse.Sample).

# License
This project uses the [MIT License](https://github.com/renanrcp/CustomAuthResponse/blob/master/LICENSE).