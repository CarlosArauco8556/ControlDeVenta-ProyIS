using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;
using Microsoft.AspNetCore.Identity;

namespace ControlDeVenta_Proy.src.Data
{
    public class DataSeeder
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "Worker", NormalizedName = "WORKER" },
                    new IdentityRole { Name = "Client", NormalizedName = "CLIENT" }
                };

                foreach (var role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
        }
    }
}