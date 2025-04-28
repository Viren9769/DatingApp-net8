using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;
using Microsoft.EntityFrameworkCore;
namespace DatingAPI.Interface
{
    public interface IUserRepository
    {
        void Update (appUser user);
        Task<bool> SaveAllAsync ();
        Task<IEnumerable<appUser>> GetUsersAsync ();
        Task <appUser?> GetUserByIdAsync(int id);
        Task<appUser?> GetUserByNameAsync (string name);

        Task<PagedList<MembersDTO>> GetAllMembersAsync (UserParams userParams);
        Task<MembersDTO?> GetMemberAsync(string username);
    }
}
