using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityRepository : IBaseRepository<Security>
    {
        Task<Security> GetLoginByCredentials(UserLogin login);
    }
}
