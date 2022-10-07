using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.Model;

namespace MyEmployees.Infrastructure.Persistence.Json;

public class ExclusiveStartKeyStringJsonConverter : JsonConverter<AttributeValue>
{
    public override AttributeValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new AttributeValue(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, AttributeValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.S);
    }
}