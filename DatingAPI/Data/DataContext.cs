using DatingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<appUser> Users { get; set; }
    }
}
