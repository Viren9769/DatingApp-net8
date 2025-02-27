using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interface;
using Microsoft.EntityFrameworkCore;


namespace DatingAPI.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public async Task<IEnumerable<MembersDTO>> GetAllMembersAsync()
        {
            return await context.Users
                .ProjectTo<MembersDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MembersDTO?> GetMemberAsync(string username)
        {
          return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MembersDTO>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<appUser?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<appUser?> GetUserByNameAsync(string name)
        {
            return await context.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<IEnumerable<appUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(appUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
