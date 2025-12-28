using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

Console.WriteLine("Aurora - Azurite Local Storage Setup");
Console.WriteLine("=====================================\n");

// Azurite connection string (well-known development storage account)
const string connectionString = "UseDevelopmentStorage=true";
const string containerName = "aurora-content";
const string blobName = "content.json";

try
{
	var blobServiceClient = new BlobServiceClient(connectionString);

	// Create container if it doesn't exist
	Console.WriteLine($"Creating container '{containerName}'...");
	var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
	await containerClient.CreateIfNotExistsAsync();
	Console.WriteLine($"✓ Container '{containerName}' ready\n");

	// Upload content.json
	var contentPath = Path.Combine("..", "..", "src", "Aurora.Api", "sample.content.json");
	if (!File.Exists(contentPath))
	{
		Console.WriteLine($"ERROR: Could not find sample.content.json at {contentPath}");
		return 1;
	}

	Console.WriteLine($"Uploading {blobName}...");
	var blobClient = containerClient.GetBlobClient(blobName);
	await blobClient.UploadAsync(contentPath, overwrite: true);
	Console.WriteLine($"✓ Uploaded {blobName} to Azurite\n");

	Console.WriteLine("Setup complete! Local Azurite storage is ready.");
	Console.WriteLine($"Blob URL: http://127.0.0.1:10000/devstoreaccount1/{containerName}/{blobName}");
	return 0;
}
catch (Exception ex)
{
	Console.WriteLine($"\nERROR: {ex.Message}");
	Console.WriteLine("\nMake sure Azurite is running. You can start it by:");
	Console.WriteLine("1. Running the Aurora.Api project in Visual Studio");
	Console.WriteLine("2. Or manually: azurite --silent");
	return 1;
}
