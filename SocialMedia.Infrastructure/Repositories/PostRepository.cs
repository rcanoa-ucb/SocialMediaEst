using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enum;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IDapperContext _dapper;
        //private readonly SocialMediaContext _context;
        public PostRepository(SocialMediaContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
            //_context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostByUserAsync(int idUser)
        {
            var posts = await _entities.Where(x => x.UserId == idUser).ToListAsync();
            return posts;
        }


        public async Task<IEnumerable<Post>> GetAllPostDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                    ",
                    DatabaseProvider.MySql => @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        LIMIT @Limit
                    ",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Post>(sql, new { Limit = limit });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        //public async Task<Post> GetPostAsync(int id)
        //{
        //    var post = await _context.Posts.FirstOrDefaultAsync(
        //        x => x.Id == id);
        //    return post;
        //}

        //public async Task InsertPostAsync(Post post)
        //{
        //    _context.Posts.Add(post);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdatePostAsync(Post post)
        //{
        //    _context.Posts.Update(post);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeletePostAsync(Post post)
        //{
        //    _context.Posts.Remove(post);
        //    await _context.SaveChangesAsync();
        //}
    }
}
