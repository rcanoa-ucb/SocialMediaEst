using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllPostsByUserAsync(int userId);
    }
}
