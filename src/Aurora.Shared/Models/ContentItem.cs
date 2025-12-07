namespace Aurora.Shared.Models
{
    public class ContentItem
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Snippet { get; set; }
        public required string ImageUrl { get; set; }
        public required string ArticleUrl { get; set; }
    }
}