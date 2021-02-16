using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

         services.AddControllersWithViews();
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseRouting();


         app.UseAuthentication();

         // Who can access?
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapDefaultControllerRoute();
         });
      }
   }
}
