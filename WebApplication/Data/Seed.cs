using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Data
{
    public static class Seed
    {
        public static void SeedData(this IApplicationBuilder app, ApplicationDbContext db)
        {
            SeedRoles(db);
            SeedUsers(db);
        }

        public static void SeedRoles(ApplicationDbContext db)
        {
            if (!db.Roles.Any(x => x.Name == "Admin"))
            {
                var roles = new[]
                {
                    new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN" },
                    new ApplicationRole { Name = "NormalUser", NormalizedName = "NORMALUSER" },
                };
                foreach (var role in roles)
                {
                    if (!db.Roles.Any(m => m.Name == role.Name))
                    {
                        db.Roles.Add(role);
                    }
                }
                db.SaveChanges();
            }
        }

        public static void SeedUsers(ApplicationDbContext db)
        {
            var role = db.Roles.FirstOrDefault(x => x.Name == "Admin");
            if (!db.UserRoles.Any(x => x.RoleId == role.Id) && !db.Users.Any(x => x.UserName == "admin"))
            {
                CreateUser(db, "admin", "admin@gmail.com", "Password@123", role.Id);
            }

            role = db.Roles.FirstOrDefault(x => x.Name == "NormalUser");
            if (!db.UserRoles.Any(x => x.RoleId == role.Id) && !db.Users.Any(x => x.UserName == "normaluser"))
            {
                CreateUser(db, "normaluser", "normaluser@gmail.com", "Password@123", role.Id);
            }
        }

        private static void CreateUser(ApplicationDbContext db, string userName, string email, string pwd, int roleId)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                FullName = userName,
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                RecordActive = true
            };
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, pwd);
            user.PasswordHash = hashed;
            db.Users.Add(user);
            db.SaveChanges();
            db.UserRoles.Add(new IdentityUserRole<int> { UserId = user.Id, RoleId = roleId });
            db.SaveChanges();
        }
    }
}
