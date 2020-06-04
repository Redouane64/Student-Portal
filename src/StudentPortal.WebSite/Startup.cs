using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentPortal.WebSite.Data;
using StudentPortal.WebSite.Models;
using StudentPortal.WebSite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using StudentPortal.WebSite.Models.ViewModels;

namespace StudentPortal.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserPermissionsService, UserPermissionsService>();

            services.AddMvc();

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MessageAuthor", config => 
                    config.AddRequirements(new IsMessageAuthorOrAdminAuthorizaationPolicy()));
                options.AddPolicy("Admin", config =>
                    config.RequireRole(ApplicationRoles.Administrators));
            });

            services.AddScoped<IAuthorizationHandler, IsMessageAuthorOrAdminPolicyHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "forum",
                    template: "Forum",
                    defaults: new { controller = "ForumCategories", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Posts}/{action=Index}/{id?}");
            });
        }
    }

    public class IsMessageAuthorOrAdminAuthorizaationPolicy : IAuthorizationRequirement
    {
    }

    public class IsMessageAuthorOrAdminPolicyHandler : AuthorizationHandler<IsMessageAuthorOrAdminAuthorizaationPolicy, int>
    {
        private readonly ApplicationDbContext dataContext;

        public IsMessageAuthorOrAdminPolicyHandler(ApplicationDbContext context)
        {
            this.dataContext = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsMessageAuthorOrAdminAuthorizaationPolicy requirement, int resource)
        {
            if(context.User.IsInRole(ApplicationRoles.Administrators))
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if(await dataContext.ForumMessages.AnyAsync(m => m.Id == resource && m.CreatorId == userId))
            {
                context.Succeed(requirement);
                return;
            }
        }

    }
}
