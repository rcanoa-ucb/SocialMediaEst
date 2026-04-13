using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync(PostQueryFilter? filters = null);
        Task<Post> GetPostByIdAsync(int id);
        Task InsertPost(Post post);
        void UpdatePost(Post post);
        Task DeletePost(int id);
    }
}
