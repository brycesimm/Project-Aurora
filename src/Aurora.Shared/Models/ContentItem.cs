using System.Text.Json.Serialization;

namespace Aurora.Shared.Models;

public class ContentItem
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("snippet")]
    public required string Snippet { get; set; }

    [JsonPropertyName("image_url")]
    public required string ImageUrl { get; set; }

    [JsonPropertyName("article_url")]
    public required string ArticleUrl { get; set; }
}