using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostAsync(PostQueryFilter postQueryFilter);
        Task<IEnumerable<Post>> GetAllPostDapperAsync();
        Task<IEnumerable<PostComentariosUsersResponse>> GetPostCommentUserAsync();
        Task<Post> GetPostAsync(int id);
        Task InsertPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
    }
}
