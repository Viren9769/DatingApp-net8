using DatingAPI.DTOs;
using DatingAPI.Entities;

namespace DatingAPI.Interface
{
    public interface IUserRepository
    {
        void Update (appUser user);
        Task<bool> SaveAllAsync ();
        Task<IEnumerable<appUser>> GetUsersAsync ();
        Task <appUser?> GetUserByIdAsync(int id);
        Task<appUser?> GetUserByNameAsync (string name);

        Task<IEnumerable<MembersDTO>> GetAllMembersAsync ();
        Task<MembersDTO?> GetMemberAsync(string username);
    }
}
