using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Comment> CommentRepository { get; }
        ISecurityRepository SecurityRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        //Nuevos miembros para Dapper
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}
