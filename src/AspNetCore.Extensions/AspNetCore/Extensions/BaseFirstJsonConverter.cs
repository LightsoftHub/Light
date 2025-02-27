using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.AspNetCore.Extensions;

public class BaseFirstJsonConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization is not implemented.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var type = value.GetType();
        writer.WriteStartObject();

        // Write Base Class Properties First (from top-most to bottom)
        foreach (var baseType in GetBaseTypes(type).Reverse())
        {
            WriteProperties(writer, value, baseType, options);
        }

        // Write Derived Class Properties
        WriteProperties(writer, value, type, options);

        writer.WriteEndObject();
    }

    private static void WriteProperties(Utf8JsonWriter writer, object value, Type type, JsonSerializerOptions options)
    {
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            object propValue = prop.GetValue(value);
            writer.WritePropertyName(prop.Name);
            JsonSerializer.Serialize(writer, propValue, options);
        }
    }

    private static Type[] GetBaseTypes(Type type)
    {
        var baseTypes = new System.Collections.Generic.List<Type>();
        var current = type.BaseType;
        while (current != null && current != typeof(object))
        {
            baseTypes.Add(current);
            current = current.BaseType;
        }
        return baseTypes.ToArray();
    }
}