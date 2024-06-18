using LearningPython.DAL.Context;
using LearningPython.DAL.Models;
using LearningPython.BLL.Data.Enums;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace LearningPython.Web
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Lessons.Any())
                {
                    context.Lessons.AddRange(new List<Lesson>()
                    {
                        new Lesson()
                        {
                            Title = "Python Basics",
                            Image = "data:image/png;base64,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",
                            Description = "Learn how to work with Python enviroment from zero to hero!",
                            LessonTag = LessonTag.Basics
                         },
                        new Lesson()
                        {
                            Title = "Stack",
                            Image = "data:image/png;base64,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",
                            Description = "Learn how to work with C# enviroment from zero to hero!",
                            LessonTag = LessonTag.Graph
                         },
                    });
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@lp.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "lpadmin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Pass123!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                //string appUserEmail = "helmytkasiyan@gmail.com";

                //var appUser = await userManager.FindByEmailAsync(appUserEmail);
                //if (appUser == null)
                //{
                //    var newAppUser = new User()
                //    {
                //        UserName = "helmyt",
                //        Email = appUserEmail,
                //        EmailConfirmed = true,
                //    };
                //    await userManager.CreateAsync(newAppUser, "Pass123!");
                //    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                //}
            }
        }
    }
}
