using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace TierLab.Application.Common;

public static class CursorSerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public static string Encode(CursorPageKey cursor)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(cursor, JsonOptions);
        return Convert.ToBase64String(bytes);
    }

    public static bool TryDecode([NotNullWhen(true)] string? encoded, [NotNullWhen(true)] out CursorPageKey? cursor)
    {
        cursor = null;
        if (string.IsNullOrWhiteSpace(encoded))
            return false;

        try
        {
            var bytes = Convert.FromBase64String(encoded);
            cursor = JsonSerializer.Deserialize<CursorPageKey>(bytes, JsonOptions);
            return cursor is not null;
        }
        catch
        {
            return false;
        }
    }
}
