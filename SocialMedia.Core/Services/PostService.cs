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
        //public readonly IPostRepository _postRepository;
        public readonly IBaseRepository<Post> _postRepository;
        //public readonly IUserRepository _userRepository;
        public readonly IBaseRepository<User> _userRepository;
        public PostService(
            //IPostRepository postRepository, 
            //IUserRepository userRepository) 
            IBaseRepository<Post> postRepository,
            IBaseRepository<User> userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            return await _postRepository.GetAll();
        }

        public async Task<Post> GetPostAsync(int id)
        {
            return await _postRepository.GetById(id);
        }

        public async Task InsertPostAsync(Post post)
        {
            var user = await _userRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (ContainsForbiddenWord(post.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            await _postRepository.Add(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.Update(post);
        }
        public async Task DeletePostAsync(int id)
        {
            await _postRepository.Delete(id);
        }

        //Lista de palabras no permitidas
        public readonly string[] ForbiddensWords =
        {
            "violencia",
            "odio",
            "groseria",
            "discriminacion"
        };

        public bool ContainsForbiddenWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            foreach (var word in ForbiddensWords)
            { 
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
