using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.Extensions.Json
{
    public class BaseFirstOrderedConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsClass && typeToConvert != typeof(string);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var converterType = typeof(BaseFirstOrderedConverter<>).MakeGenericType(type);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }

    public class BaseFirstOrderedConverter<T> : JsonConverter<T>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Fallback to default deserialization
            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Gather base-to-derived properties
            var type = typeof(T);
            var types = new Stack<Type>();
            while (type != null && type != typeof(object))
            {
                types.Push(type);
                type = type.BaseType!;
            }

            var allProps = new List<PropertyInfo>();
            while (types.Count > 0)
            {
                var currentType = types.Pop();
                var props = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .OrderBy(p => p.GetCustomAttribute<JsonPropertyOrderAttribute>()?.Order ?? 0);
                allProps.AddRange(props);
            }

            writer.WriteStartObject();

            foreach (var prop in allProps)
            {
                var propValue = prop.GetValue(value);
                var propName = options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name;

                writer.WritePropertyName(propName);
                JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
            }

            writer.WriteEndObject();
        }
    }
}
