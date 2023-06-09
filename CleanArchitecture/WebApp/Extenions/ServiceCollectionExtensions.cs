﻿using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace WebApp.Extenions
{
	public static class ServiceCollectionExtensions
	{
		internal static void RegisterAzureAuthDependencies(this IServiceCollection services, IConfiguration configuration) 
		{
            if (!configuration.GetValue<bool>("ApiClientConfig:Authentication"))
                services.AddRazorPages();
            else
            {
                var initialScopes = configuration["ApiClientConfig:Scopes"]?.Split(' ') ?? configuration["MicrosoftGraph:Scopes"]?.Split(' ');
                services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
                        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                            .AddDownstreamWebApi("ApiClientConfig", configuration.GetSection("ApiClientConfig"))
                            .AddInMemoryTokenCaches();
                services.AddRazorPages().AddMvcOptions(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }).AddMicrosoftIdentityUI();
            }
        }
	}
}
