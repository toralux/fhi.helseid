﻿using Fhi.HelseId.Common.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Fhi.HelseId.Api
{
    public static class HelseIdOptionsExtensions
    {

        public static AuthenticationBuilder AddHelseIdJwtBearer(this AuthenticationBuilder authenticationBuilder,
            IHelseIdApiKonfigurasjon configAuth)
        {
            var builder = authenticationBuilder.AddJwtBearer(
                options =>
                {
                    options.Authority = configAuth.Authority;
                    options.Audience = configAuth.ApiName;
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.RefreshOnIssuerKeyNotFound = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireSignedTokens = true,
                        RequireAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                    };
                }
            );
            return builder;
        }

        /// <summary>
        /// This adds policies for requiring caller to be an user with an authenticated Identity.  Note that this might also be something else than HelseId, and might be confusing.
        /// This policy can not be used with Worker processes.
        /// This policy also adds the Api scope requirement
        /// </summary>
        public static void AddHelseIdAuthorization(this IServiceCollection services, IHelseIdApiKonfigurasjon configAuth)
        {
            services.AddAuthorization(
                config =>
                {
                    var authenticatedHidUserPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    var apiAccessPolicy = new AuthorizationPolicyBuilder()
                        .Combine(authenticatedHidUserPolicy)
                        .RequireScope(configAuth.ApiScope)
                        .Build();

                    config.DefaultPolicy = apiAccessPolicy;

                    config.AddPolicy(Policies.HidAuthenticated, authenticatedHidUserPolicy);
                    config.AddPolicy(Policies.ApiAccess, apiAccessPolicy);
                }
            );
        }

        /// <summary>
        /// This policy only adds the Api scope requirement
        /// </summary>
        public static void AddHelseIdApiAccessOnlyAuthorization(this IServiceCollection services, IHelseIdApiKonfigurasjon configAuth)
        {
            services.AddAuthorization(
                config =>
                {
                    var apiAccessPolicy = new AuthorizationPolicyBuilder()
                        .RequireScope(configAuth.ApiScope)
                        .Build();

                    config.DefaultPolicy = apiAccessPolicy;

                    config.AddPolicy(Policies.ApiAccess, apiAccessPolicy);
                }
            );
        }

    }
}
