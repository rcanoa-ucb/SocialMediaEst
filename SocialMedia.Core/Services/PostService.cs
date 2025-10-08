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
        public readonly IUserRepository _userRepository;
        public PostService(IPostRepository postRepository, 
            IUserRepository userRepository) 
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
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
            var user = await _userRepository.GetUserByIdAsync(post.UserId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (post.Description.ToLower().Contains("odio".ToLower()))
            {
                throw new Exception("El contenido no es permitido");
            }

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
