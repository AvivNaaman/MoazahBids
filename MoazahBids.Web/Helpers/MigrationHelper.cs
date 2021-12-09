using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoazahBids.Web.Data;
using MoazahBids.Web.Models;
using System;
using System.Threading.Tasks;

namespace MoazahBids.Web.Helpers
{
    public class MigrationHelper
    {
        private readonly BidsDbContext dbCtx;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IOptions<AppConfig> options;

        public MigrationHelper(BidsDbContext dbCtx, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppConfig> options)
        {
            this.dbCtx = dbCtx;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.options = options;
        }

        public async Task Migrate()
        {
            await dbCtx.Database.MigrateAsync();

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var r = await roleManager.CreateAsync(new("Admin"));
            }

            foreach (var u in options.Value.Admins)
            {
                IdentityResult r;
                var newu = (await userManager.FindByNameAsync(u.UserName)) ?? (await userManager.FindByEmailAsync(u.Email));
                if (newu is null)
                {
                    newu = new ApplicationUser
                    {
                        UserName = u.UserName,
                        Email = u.Email
                    };
                    r = await userManager.CreateAsync(newu, u.Password);
                }
                r = await userManager.AddToRoleAsync(newu, "Admin");
            }
        }
    }
}