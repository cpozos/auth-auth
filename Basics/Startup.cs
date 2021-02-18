using Basics.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Basics
{
   public class Startup
   {

      public void ConfigureServices(IServiceCollection services)
      {
         string name = "CookieAuth";
         services.AddAuthentication(name)
            .AddCookie(name, config =>
            {
               config.Cookie.Name = "NameCookie";
               config.LoginPath = "/Home/Authenticate";
            });


         services.AddAuthorization(config =>
         {
            //// This is what happens under the hoods
            //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //var defaultAuthPolicy = defaultAuthBuilder
            //   .RequireAuthenticatedUser()
            //   .RequireClaim(ClaimTypes.DateOfBirth)
            //   .Build();

            //config.DefaultPolicy = defaultAuthPolicy;

            config.AddPolicy("Claim.DayOfBirth", policyBuilder =>
            {
               policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
            });
         });

         services.AddControllersWithViews();

         // Injections
         services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseRouting();

         // Who is requesting? Who are you?
         app.UseAuthentication();

         // Who can access? Are you allowed?
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapDefaultControllerRoute();
         });
      }
   }
}
