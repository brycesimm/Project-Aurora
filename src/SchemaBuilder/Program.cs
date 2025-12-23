using Aurora.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SchemaBuilder;
using Swashbuckle.AspNetCore.SwaggerGen;

Console.WriteLine("Starting schema generation...");

// 1. Set up a minimal service provider with Swagger services
var services = new ServiceCollection();
services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Aurora Schema", Version = "v1" });
});

var serviceProvider = services.BuildServiceProvider();
var schemaGenerator = serviceProvider.GetRequiredService<ISchemaGenerator>();

// 2. Generate the OpenAPI schema for the ContentItem type
var repository = new SchemaRepository();
schemaGenerator.GenerateSchema(typeof(ContentItem), repository);
var schema = repository.Schemas["ContentItem"]; // Retrieve the full schema from the repository
schema.Required = new HashSet<string>(schema.Properties.Keys); // Manually mark all properties as required


// 3. Serialize the generated schema to a JSON string using Newtonsoft.Json
var settings = new JsonSerializerSettings
{
	Formatting = Formatting.Indented,
	NullValueHandling = NullValueHandling.Ignore,
	Converters = { new OpenApiSchemaJsonConverter() }
};

var schemaJson = JsonConvert.SerializeObject(schema, settings);

// 4. Define the output path and write the file
var solutionRoot = FindSolutionRoot(AppContext.BaseDirectory);
if (solutionRoot == null)
{
	Console.Error.WriteLine("Error: Could not find the solution root directory (containing Project-Aurora.sln).");
	return 1;
}

var outputPath = Path.Combine(solutionRoot, "content.schema.json");
File.WriteAllText(outputPath, schemaJson);

Console.WriteLine($@"Schema successfully generated and saved to: {Path.GetFullPath(outputPath)}");

return 0;

static string? FindSolutionRoot(string startPath)
{
	var currentPath = new DirectoryInfo(startPath);
	while (currentPath != null && currentPath.GetFiles("Project-Aurora.sln").Length == 0)
	{
		currentPath = currentPath.Parent;
	}
	return currentPath?.FullName;
}
