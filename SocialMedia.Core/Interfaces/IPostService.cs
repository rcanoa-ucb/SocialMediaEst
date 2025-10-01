using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostAsync();
        Task<Post> GetPostAsync(int id);
        Task InsertPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);
    }
}
