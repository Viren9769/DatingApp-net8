using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data; // File-scoped namespace (recommended in .NET 6+)

public static class Seed
{
    public static async Task Seeduser(UserManager<appUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userdata = await File.ReadAllTextAsync("C:\\Users\\Viren\\Desktop\\Authentication\\DatingAPI\\DatingAPI\\Data\\UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<appUser>>(userdata, options);
        if (users == null) return;
        var roles = new List<AppRole>
        {
            new() {Name ="Member"},
            new() {Name ="Admin"},
            new() {Name ="Moderator"},
        };
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLower(); 
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new appUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",
            City = "",
            Country = ""
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
}
