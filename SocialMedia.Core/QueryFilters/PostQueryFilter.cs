namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter : PaginationQueryFilter
    {
        public int? UserId { get; set; }
        public string? Date { get; set; }
        public string Description { get; set; }
    }
}
