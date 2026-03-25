using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
    }
}
