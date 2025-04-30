using Light.Contracts;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.Extensions.Json
{
    public class PropertyOrderedConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return
                typeToConvert.IsClass
                && !typeToConvert.IsAbstract
                && typeToConvert.GetProperties()
                    .Any(p => p.GetCustomAttribute<PropertyOrderAttribute>() != null);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var converterType = typeof(PropertyOrderedConverter<>).MakeGenericType(type);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }

    public class PropertyOrderedConverter<T> : JsonConverter<T> where T : class
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options)!;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var orderedProps = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(p =>
                {
                    var attr = p.GetCustomAttribute<PropertyOrderAttribute>();
                    return attr?.Order ?? 0;
                });

            writer.WriteStartObject();

            foreach (var prop in orderedProps)
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
