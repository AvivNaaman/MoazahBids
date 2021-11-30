using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoazahBids.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoazahBids.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MoazahBids.Web
{
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
            services.AddAntiforgery();
            services.AddRazorPages();


            services.AddDbContext<BidsDbContext>(db => db.UseSqlite(Configuration.GetConnectionString("Bids")));

            services.AddScoped<IEmailSender, FakeEmailSender>();

            services.AddIdentity<ApplicationUser, IdentityRole>(identity =>
            {
                identity.SignIn.RequireConfirmedEmail = false;
                identity.SignIn.RequireConfirmedPhoneNumber = false;

                identity.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BidsDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IBidsService, BidsService>();

            services.ConfigureApplicationCookie(cookie =>
            {
                cookie.LoginPath = "/Identity/Account/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
