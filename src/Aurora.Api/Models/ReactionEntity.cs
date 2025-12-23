using Azure;
using Azure.Data.Tables;

namespace Aurora.Api.Models;

public class ReactionEntity : ITableEntity
{
	public string PartitionKey { get; set; } = "Content";
	public string RowKey { get; set; } = string.Empty;
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }

	public int UpliftCount { get; set; }
}
