using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {             // Seed Roles
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            }; builder.Entity<IdentityRole>().HasData(roles);             // -----------------------------------------------------------------------------
            // Seed Users
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            string UserName = "test@example.com";
            string AdminUserName = "admin@example.com";
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser {
                    UserName = UserName,
                    NormalizedUserName = UserName.ToUpper(),
                    Email = UserName,
                    NormalizedEmail = UserName.ToUpper(),
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = "User",
                    Height = 72,
                    Gender = 1,
                    ActivityLevel = 1,
                    BirthDay = DateTime.Now
                },
                new ApplicationUser {
                    UserName = AdminUserName,
                    NormalizedUserName = AdminUserName.ToUpper(),
                    Email = AdminUserName,
                    NormalizedEmail = AdminUserName.ToUpper(),
                    EmailConfirmed = true,
                }
            }; builder.Entity<ApplicationUser>().HasData(users);
            ///----------------------------------------------------
            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();             // Add Password For All Users
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "P@55w0rd");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "P@55w0rd");
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(q => q.Name == "User").Id
            });
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[1].Id,
                RoleId = roles.First(q => q.Name == "Admin").Id
            });
            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}
