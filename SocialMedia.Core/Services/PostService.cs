using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        public readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository) 
        {
            _postRepository = postRepository;
        }
        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            return await _postRepository.GetAllPostAsync();
        }

        public async Task<Post> GetPostAsync(int id)
        {
            return await _postRepository.GetPostAsync(id);
        }

        public async Task InsertPostAsync(Post post)
        {
            await _postRepository.InsertPostAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdatePostAsync(post);
        }
        public async Task DeletePostAsync(Post post)
        {
            await _postRepository.DeletePostAsync(post);
        }
    }
}
