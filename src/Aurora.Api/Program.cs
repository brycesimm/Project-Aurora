using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Aurora.Api.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
	.AddApplicationInsightsTelemetryWorkerService()
	.ConfigureFunctionsApplicationInsights()
	.AddSingleton(sp =>
	{
		var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? "UseDevelopmentStorage=true";
		return new TableServiceClient(connectionString);
	})
	.AddSingleton(sp =>
	{
		var connectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString") ?? "UseDevelopmentStorage=true";
		return new BlobServiceClient(connectionString);
	})
	.AddSingleton<ReactionStorageService>();

builder.Build().Run();
