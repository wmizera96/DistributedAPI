using System.Text.Json;

namespace DistributedAPI.TestTools.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> GetContentAsync<T>(this HttpResponseMessage message)
    {
        var stringContent = await message.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}