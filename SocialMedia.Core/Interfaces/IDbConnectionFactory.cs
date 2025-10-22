using SocialMedia.Core.Enum;
using System.Data;

namespace SocialMedia.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}
