using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Napoleon.Shared.Common;

public static class HelperClass
{
    public static bool IsNullOrEmpty<TType>([NotNullWhen(true)] this IList<TType>? data)
    {
        return data is null || !data.Any();
    }
    public static bool IsNullOrEmpty([NotNullWhen(true)] this string? data)
    {
        return string.IsNullOrWhiteSpace(data);
    }
    
    public static bool IsNotNullOrEmpty<TType>([NotNullWhen(false)] this IList<TType>? data)
    {
        return !data.IsNullOrEmpty();
    }

    
    public static bool IsNotNullOrEmpty([NotNullWhen(false)]  this string? data)
    {
        return !data.IsNullOrEmpty();
    }

    public static T Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized)!;
    }

    public static string ToJson(this object source)
    {
        return JsonConvert.SerializeObject(source);
    }
}

