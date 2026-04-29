using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagedList<Post>> GetAllPostsAsync(PostQueryFilter? filters = null);
        Task<IEnumerable<Post>> GetAllPostsDapperAsync(int limit = 10);
        Task<Post> GetPostByIdAsync(int id);
        Task InsertPost(Post post);
        void UpdatePost(Post post);
        Task DeletePost(int id);
    }
}
