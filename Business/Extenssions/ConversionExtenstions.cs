using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Microsoft.EntityFrameworkCore;

public static class ConversionsExtensions
{
    public static PropertyBuilder<TProperty[]> HasArrayToJsonConversion<TProperty>(this PropertyBuilder<TProperty[]> builder)
    {
        return builder.HasConversion((TProperty[] value) => Serialize(value), (string data) => Deserialize<TProperty>(data));
    }

    public static PropertyBuilder<HashSet<TProperty>> HasArrayToJsonConversion<TProperty>(this PropertyBuilder<HashSet<TProperty>> builder)
    {
        return builder.HasConversion((HashSet<TProperty> value) => Serialize(value.ToArray()), (string data) => new HashSet<TProperty>(Deserialize<TProperty>(data)));
    }

    public static PropertyBuilder<IEnumerable<TProperty>> HasArrayToJsonConversion<TProperty>(this PropertyBuilder<IEnumerable<TProperty>> builder)
    {
        return builder.HasConversion((IEnumerable<TProperty> value) => Serialize(value.ToArray()), (string data) => Deserialize<TProperty>(data));
    }

    private static string Serialize<T>(T[] value)
    {
        return JsonSerializer.Serialize(value);
    }

    private static T[] Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T[]>(value) ?? Array.Empty<T>();
    }
}
