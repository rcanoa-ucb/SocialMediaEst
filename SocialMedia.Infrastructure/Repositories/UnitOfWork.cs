using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext _context;
        public readonly IBaseRepository<Post> _postRepository;
        public readonly IBaseRepository<User> _userRepository;
        public readonly IBaseRepository<Comment> _commentRepository;

        public UnitOfWork(SocialMediaContext context)
        {
            _context = context;
        }

        public IBaseRepository<Post> PostRepository =>
            _postRepository ?? new BaseRepository<Post>(_context);

        public IBaseRepository<User> UserRepository =>
           _userRepository ?? new BaseRepository<User>(_context);

        public IBaseRepository<Comment> CommentRepository =>
            _commentRepository ?? new BaseRepository<Comment>(_context);

        public void Dispose()
        {
            if (_context != null)
            { 
                _context.Dispose();
            }
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
