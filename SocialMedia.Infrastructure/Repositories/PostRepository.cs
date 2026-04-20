using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enum;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Queries;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IDapperContext _dapper;

        public PostRepository(SocialMediaContext context,
            IDapperContext dapper) 
            : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Post>> GetAllPostsByUserAsync(int userId)
        {
            return await _entities
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>>
            GetAllPostDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.SqlServer => Primero.unoSql,
                    DataBaseProvider.MySql => Primero.unoMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Post>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
