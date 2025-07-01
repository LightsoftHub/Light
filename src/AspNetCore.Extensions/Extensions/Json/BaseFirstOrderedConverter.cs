using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.Extensions.Json;

public class BaseFirstOrderedConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsClass && typeToConvert != typeof(string);
    }

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var converterType = typeof(BaseFirstOrderedConverter<>).MakeGenericType(type);
        return (JsonConverter)Activator.CreateInstance(converterType, options)!;
    }
}

public class BaseFirstOrderedConverter<T>(JsonSerializerOptions options) : OrderedConverterBase<T>(options)
    where T : class
{
    public override IEnumerable<PropertyInfo> GetPropertyInfos()
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
                .Where(p => p.GetIndexParameters().Length == 0)
                .OrderBy(p => p.GetCustomAttribute<JsonPropertyOrderAttribute>()?.Order ?? 0);
            allProps.AddRange(props);
        }

        return allProps;
    }
}
