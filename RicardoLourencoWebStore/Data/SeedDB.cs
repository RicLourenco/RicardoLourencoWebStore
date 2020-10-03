using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data
{
    public class SeedDB
    {
        readonly DataContext _context;
        readonly IUserHelper _userHelper;

        public SeedDB(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRolesAsync("Admin");
            await _userHelper.CheckRolesAsync("Client");

            var user = await _userHelper.GetUserByEmailAsync("ricardo.pinto.lourenco@formandos.cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstNames = "Ricardo Filipe",
                    LastNames = "Pinto Lourenço",
                    Email = "ricardo.pinto.lourenco@formandos.cinel.pt",
                    UserName = "ricardo.pinto.lourenco@formandos.cinel.pt",
                    PhoneNumber = "912345678",
                    Address = "Rua da Luz 23, 2dto.",
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "ABab12!?");

                await _userHelper.AddUserToRoleAsync(user, "Admin");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            var category = await _context.Categories.Where(c => c.Name == "Console").FirstOrDefaultAsync();

            if (category == null)
            {
                var entity = new Category
                {
                    Name = "Console"
                };

                await _context.Categories.AddAsync(entity);
                await _context.SaveChangesAsync();

                category = await _context.Categories.Where(c => c.Name == "Console").FirstOrDefaultAsync();
            }

            if (!_context.Products.Any())
            {
                AddProduct("Consola XPTO", 300, 30, category);
                AddProduct("Consola ABC", 300, 30, category);
                AddProduct("Consola DEFG", 300, 30, category);
                await _context.SaveChangesAsync();
            }
        }

        void AddProduct(string name, float price, int stock, Category category)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = price,
                IsAvailable = true,
                Stock = stock,
                Category = category
            });
        }
    }
}
