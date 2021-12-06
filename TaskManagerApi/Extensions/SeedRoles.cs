using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Infrastructure;

namespace TaskManagerApi.Extensions
{
    public class SeedRoles
    {
        private TaskManagerDB _context;

        public SeedRoles(TaskManagerDB context)
        {
            _context = context;
        }

        public async void SeedDefaultRoles()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            }
            if (!_context.Roles.Any(r => r.Name == "user"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "user", NormalizedName = "user" });
            }
            await _context.SaveChangesAsync();

        }
    }
}
