using Microsoft.AspNetCore.Identity;
using TeleDoc.API.Static;
using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Extensions;

namespace TeleDoc.API.Data;

public static class AppDbInitializer
{
    public static async Task SeedUsersAndRoleAsync(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            
        if (!await roleManager.RoleExistsAsync(UserRoles.Doctor))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));
            
        if (!await roleManager.RoleExistsAsync(UserRoles.Patient))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Patient));

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string adminEmail = "admin@email.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            var newAdmin = new ApplicationUser()
            {
                Name = "Admin User",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(newAdmin, "pass123");
            await userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
        }
        
        const string doctortEmail = "doctor@email.com";
        var doctortUser = await userManager.FindByEmailAsync(doctortEmail);
        if (doctortUser is null)
        {
            var newDoctor = new ApplicationUser()
            {
                Name = "Doctor User",
                UserName = doctortEmail,
                Email = doctortEmail,
                Gender = UserGender.Female,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(newDoctor, "pass123");
            await userManager.AddToRoleAsync(newDoctor, UserRoles.Patient);
        }
            
            
        const string patientEmail = "patient@email.com";
        var patientUser = await userManager.FindByEmailAsync(patientEmail);
        if (patientUser is null)
        {
            var newPatient = new ApplicationUser()
            {
                Name = "Patient User",
                UserName = patientEmail,
                Email = patientEmail,
                Gender = UserGender.Male,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(newPatient, "pass123");
            await userManager.AddToRoleAsync(newPatient, UserRoles.Patient);
        }
    }
}