using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data; // File-scoped namespace (recommended in .NET 6+)

public static class Seed
{
    public static async Task Seeduser(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userdata = await File.ReadAllTextAsync("C:\\Users\\Viren\\Desktop\\Authentication\\DatingAPI\\DatingAPI\\Data\\UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<appUser>>(userdata, options);
        if (users == null) return;

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            // Ensure each photo has a proper UserId assigned
            if (user.Photos != null && user.Photos.Count > 0)
            {
                foreach (var photo in user.Photos)
                {
                    photo.appUser = user; // Ensure foreign key relationship is established
                    Console.WriteLine($"Seeding Photo: {photo.Url} for User: {user.UserName}");
                }
            }

            context.Users.Add(user);
        }

        await context.SaveChangesAsync(); // Save changes after all users are added
    }
}
