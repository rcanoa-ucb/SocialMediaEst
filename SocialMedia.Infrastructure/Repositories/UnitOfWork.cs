using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext _context;
        //private readonly IBaseRepository<Post> _postRepository;
        private readonly IPostRepository? _postRepository;
        private readonly IBaseRepository<User>? _userRepository;
        private readonly IBaseRepository<Comment>? _commentRepository;

        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(SocialMediaContext context,
            IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }


        //public IBaseRepository<Post> PostRepository =>
        //    _postRepository ?? new BaseRepository<Post>(_context);

        public IPostRepository PostRepository =>
            _postRepository ?? new PostRepository(_context, _dapper);

        public IBaseRepository<User> UserRepository => 
            _userRepository ?? new BaseRepository<User>(_context);

        public IBaseRepository<Comment> CommentRepository => 
            _commentRepository ?? new BaseRepository<Comment>(_context);

        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                //Registrar la coneccion/Tx en DapperContext}
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _efTransaction?.Dispose();
            }
        }

        public IDbConnection? GetDbConnection()
        {
            // Retornamos la conexión subyacente del DbContext
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
