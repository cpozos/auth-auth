using Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;

namespace Identity
{
   public class Startup
   {
      private readonly IConfiguration _config;

      public Startup(IConfiguration configuration)
      {
         _config = configuration;
      }

      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<AppDbContext>(config =>
         {
            config.UseInMemoryDatabase("Memory");
         });

         // AddIdentity registers the services
         services.AddIdentity<IdentityUser, IdentityRole>(config =>
         {
            // Remove restrictions
            config.Password.RequiredLength = 4;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
            config.SignIn.RequireConfirmedEmail = true;
         })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

         services.AddMailKit(config =>
         {
            var mailKitOptions = _config.GetSection("Email").Get<MailKitOptions>();
            config.UseMailKit(mailKitOptions);
         });

         services.ConfigureApplicationCookie(config =>
         {
            config.Cookie.Name = "NameCookie";
            config.LoginPath = "/Home/Login";
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
