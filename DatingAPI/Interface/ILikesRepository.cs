using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;

namespace DatingAPI.Interface
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceuserId, int targetUserId);
        Task<PagedList<MembersDTO>> GetUserLikes(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();
    }
}

