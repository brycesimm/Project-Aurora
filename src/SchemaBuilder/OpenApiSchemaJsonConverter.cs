using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchemaBuilder;

public class OpenApiSchemaJsonConverter : JsonConverter<OpenApiSchema>
{
    public override void WriteJson(JsonWriter writer, OpenApiSchema? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        // Write simple properties if they are not null or default
        if (value.Type != null)
        {
            writer.WritePropertyName("type");
            writer.WriteValue(value.Type);
        }
        
        // For non-string types, write "nullable: true" if the property is nullable.
        // For string types, we will omit it, making them non-nullable by default.
        if (value.Nullable && value.Type != "string")
        {
            writer.WritePropertyName("nullable");
            writer.WriteValue(value.Nullable);
        }
        
        if (!value.AdditionalPropertiesAllowed)
        {
            writer.WritePropertyName("additionalProperties");
            writer.WriteValue(value.AdditionalPropertiesAllowed);
        }

        // Write collection properties only if they are not null and contain elements
        if (value.Required != null && value.Required.Any())
        {
            writer.WritePropertyName("required");
            serializer.Serialize(writer, value.Required);
        }

        if (value.Properties != null && value.Properties.Any())
        {
            writer.WritePropertyName("properties");
            serializer.Serialize(writer, value.Properties);
        }

        writer.WriteEndObject();
    }

    public override OpenApiSchema ReadJson(JsonReader reader, Type objectType, OpenApiSchema? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // We only need this for serialization.
        throw new NotImplementedException();
    }
}
