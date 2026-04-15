using SocialMedia.Core.Enum;
using System.Data;

namespace SocialMedia.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DataBaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}
