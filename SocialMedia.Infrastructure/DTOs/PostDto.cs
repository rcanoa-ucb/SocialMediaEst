namespace SocialMedia.Infrastructure.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public string? Imagen { get; set; }
    }
}
