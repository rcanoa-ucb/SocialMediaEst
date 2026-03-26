using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services.Services
{
    public class PostService : IPostService
    {
        //public readonly IPostRepository _postRepository;
        //public readonly IUserRepository _userRepository;

        public readonly IBaseRepository<Post> _postRepository;
        public readonly IBaseRepository<User> _userRepository;

        public PostService(IBaseRepository<Post> postRepository,
            IBaseRepository<User> userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAll();
                //GetAllPostsAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetById(id);
            // GetPostByIdAsync(id);
        }

        public async Task InsertPost(Post post)
        {
            var user = await _userRepository.GetById(post.UserId);
            if (user == null)
                throw new Exception("El usuario no existe");

            if (ContainsFobbidenWord(post.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            await _postRepository.Add(post);
        }

        public async Task UpdatePost(Post post)
        {
            await _postRepository.Update(post);
        }

        public async Task DeletePost(int id)
        {
            await _postRepository.Delete(id);
        }

        //Lista de palabras o expresiones no permitidas
        public readonly string[] ForbbidenWords =
        {
            "violencia",
            "odio",
            "groseria",
            "discriminacion",
            "pornografía"
        };

        public bool ContainsFobbidenWord(string text)
        { 
            if (string.IsNullOrWhiteSpace(text))
                return false;

            foreach (var word in ForbbidenWords)
            { 
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
