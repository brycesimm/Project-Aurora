using Azure.Data.Tables;
using Aurora.Api.Models;

namespace Aurora.Api.Services;

public class ReactionStorageService
{
	private const string TableName = "Reactions";
	private readonly TableClient _tableClient;

	public ReactionStorageService(TableServiceClient tableServiceClient)
	{
		ArgumentNullException.ThrowIfNull(tableServiceClient);
		_tableClient = tableServiceClient.GetTableClient(TableName);
		_tableClient.CreateIfNotExists();
	}

	public async Task<int> GetUpliftCountAsync(string articleId)
	{
		try
		{
			var response = await _tableClient.GetEntityAsync<ReactionEntity>("Content", articleId).ConfigureAwait(false);
			return response.Value.UpliftCount;
		}
		catch (Azure.RequestFailedException ex) when (ex.Status == 404)
		{
			return 0;
		}
	}

	public async Task<int> IncrementUpliftCountAsync(string articleId)
	{
		// Simple optimistic concurrency or just blind upsert for now?
		// For accurate counters, we should read-modify-write.
		// Since we expect low traffic for MVP, we'll do simple read-modify-write.

		try
		{
			ReactionEntity entity;
			try
			{
				var response = await _tableClient.GetEntityAsync<ReactionEntity>("Content", articleId).ConfigureAwait(false);
				entity = response.Value;
			}
			catch (Azure.RequestFailedException ex) when (ex.Status == 404)
			{
				entity = new ReactionEntity { RowKey = articleId, UpliftCount = 0 };
			}

			entity.UpliftCount++;
			await _tableClient.UpsertEntityAsync(entity).ConfigureAwait(false);
			return entity.UpliftCount;
		}
		catch (Exception)
		{
			throw;
		}
	}
}
