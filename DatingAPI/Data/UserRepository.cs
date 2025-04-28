using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;


namespace DatingAPI.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public async Task<PagedList<MembersDTO>> GetAllMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();
            query = query.Where(x => x.UserName != userParams.CurrentUsername);
            if (!string.IsNullOrEmpty(userParams.Gender))
            {
                query = query.Where(x => x.Gender == userParams.Gender);
            }

            // Correct way to filter by DateOnly for Age
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var minDob = today.AddYears(-userParams.MaxAge - 1).AddDays(1); // Oldest date (MaxAge)
            var maxDob = today.AddYears(-userParams.MinAge);                // Youngest date (MinAge)

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };


            return await PagedList<MembersDTO>.CreateAsync(query.ProjectTo<MembersDTO>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
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
