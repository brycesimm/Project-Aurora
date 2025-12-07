using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add a simple endpoint to expose the ContentItem model for schema generation
app.MapGet("/schema/contentitem", () => new Aurora.Shared.Models.ContentItem()
{
    Id = "mock-id",
    Title = "Mock Title",
    Snippet = "This is a mock snippet for a positive news story.",
    ImageUrl = "https://example.com/mock-image.jpg",
    ArticleUrl = "https://example.com/mock-article"
})
    .WithOpenApi();

app.MapGet("/diagnostics/openapi-version", () =>
{
    var openApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
                                    .FirstOrDefault(a => a.GetName().Name == "Microsoft.OpenApi");

    if (openApiAssembly != null)
    {
        return Results.Ok($"Microsoft.OpenApi Assembly Version: {openApiAssembly.GetName().Version}");
    }
    return Results.NotFound("Microsoft.OpenApi assembly not found.");
});

app.Run();
