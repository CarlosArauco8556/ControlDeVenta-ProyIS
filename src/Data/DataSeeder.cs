using Bogus;
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
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var faker = new Faker();

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

                if (context.InvoiceStates.Count() == 0)
                {
                    var invoiceStates = new List<InvoiceState>
                    {
                        new InvoiceState { Name = "Pendiente" },
                        new InvoiceState { Name = "Entregada" },
                        new InvoiceState { Name = "Cancelada" }
                    };

                    context.InvoiceStates.AddRange(invoiceStates);
                }

                if (context.PaymentMethods.Count() == 0)
                {
                    var paymentMethods = new List<PaymentMethod>
                    {
                        new PaymentMethod { Name = "Efectivo" },
                        new PaymentMethod { Name = "Transferenncia" },
                    };

                    context.PaymentMethods.AddRange(paymentMethods);
                }

                if (!context.Products.Any())
                {
                    var ProductFaker = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Price, f => f.Random.Number(1000, 10000000))
                        .RuleFor(p => p.Stock, f => f.Random.Number(0, 100000))
                        .RuleFor(p => p.StockMin, f => f.Random.Number(0, 1000))
                        .RuleFor(p => p.DiscountPercentage, f => f.Random.Number(0, 100));
                    
                    var products = ProductFaker.Generate(10);
                    context.Products.AddRange(products);
                }

                context.SaveChanges();

                if (!userManager.Users.Any())
                {
                    var admin = new AppUser
                    {
                        UserName = "admin",
                        Email = "admin@idwm.cl",
                        Name = "Admin",
                        Rut = "11111111-1",
                        PhoneNumber = "111111111"
                    };

                    var adminResult = await userManager.CreateAsync(admin, "Admin1234.");

                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                    else
                    {
                        foreach (var error in adminResult.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                    
                    var workers = new List<AppUser>
                    {
                        new AppUser
                        {
                            UserName = "Jhon",
                            Email = "jhon@ucn.com",
                            Name = "Jhon",
                            Rut = "11111111-1",
                            PhoneNumber = "111111111"
                        },

                        new AppUser
                        {
                            UserName = "carlos",
                            Email = "carlo@qucn.com",
                            Name = "Carlos",
                            Rut = "22222222-2",
                            PhoneNumber = "222222222"
                        },

                        new AppUser
                        {
                            UserName = "Dantte",
                            Email = "dantte@ucn.com",
                            Name = "Dantte",
                            Rut = "33333333-3",
                            PhoneNumber = "333333333"
                        }

                    };

                    foreach (var worker in workers)
                    {
                        var workerResult = await userManager.CreateAsync(worker, "Worker1234.");

                        if (workerResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(worker, "Worker");
                        }
                        else
                        {
                            foreach (var error in workerResult.Errors)
                            {
                                Console.WriteLine($"Error: {error.Description}");
                            }
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var email = faker.Internet.Email();

                        var rut = faker.Random.Number(10000000, 99999999).ToString() + "-" + faker.Random.Number(0, 9).ToString();
                        
                        var client = new AppUser
                        {
                            UserName = email,
                            Email = email,
                            Name = faker.Name.FullName(),
                            Rut = rut,
                            PhoneNumber = faker.Phone.PhoneNumber()
                        };
                    
                        var createUser = await userManager.CreateAsync(client);

                        if (!createUser.Succeeded)
                        {
                            throw new Exception("Error al crear el usuario");
                        }

                        var roleResult = userManager.AddToRoleAsync(client, "Client");

                        if (roleResult.Result.Succeeded)
                        {
                            Console.WriteLine($"Usuario {client.Email} creado exitosamente");
                        }
                        else
                        {
                            throw new Exception("Error al asignar rol al usuario");
                        }
                    }
                }
            }
        }
    }
}