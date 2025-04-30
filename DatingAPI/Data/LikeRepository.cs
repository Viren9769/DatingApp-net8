using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class LikeRepository(DataContext context, IMapper mapper) : ILikesRepository
    {
        public void AddLike(UserLike like)
        {
            context.Likes.Add(like);
        }

        public void DeleteLike(UserLike like)
        {
            context.Likes.Remove(like);
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
        {
            return await context.Likes
                .Where(x => x.SourceuserId == currentUserId)
                .Select(x => x.TargetuserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetUserLike(int sourceuserId, int targetUserId)
        {
            return await context.Likes.FindAsync(sourceuserId, targetUserId);
        }

        public async Task<PagedList<MembersDTO>> GetUserLikes(LikesParams likesParams)
        {
            var likes = context.Likes.AsQueryable();
            IQueryable<MembersDTO> query;
            switch(likesParams.Predicate)
            {
                case "liked":
                    query = likes
                        .Where(x => x.SourceuserId == likesParams.UserId)
                        .Select(x => x.TargetUser)
                        .ProjectTo<MembersDTO>(mapper.ConfigurationProvider);
                        break;
                case "likedBy":
                    query = likes
                        .Where(x => x.TargetuserId == likesParams.UserId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MembersDTO>(mapper.ConfigurationProvider);
                        break;
                default:
                    var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);

                    query = likes
                        .Where(x => x.TargetuserId == likesParams.UserId && likeIds.Contains(x.SourceuserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MembersDTO>(mapper.ConfigurationProvider);
                        break;
                     

            }
            return await PagedList<MembersDTO>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
