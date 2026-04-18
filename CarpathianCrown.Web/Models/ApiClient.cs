using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarpathianCrown.Web.Models;

public sealed class ApiClient
{
    private readonly IHttpClientFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public ApiClient(IHttpClientFactory factory) => _factory = factory;

    private HttpClient Create(string? token)
    {
        var c = _factory.CreateClient("api");
        if (!string.IsNullOrWhiteSpace(token))
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return c;
    }

    private static StringContent JsonBody<T>(T obj)
        => new(JsonSerializer.Serialize(obj, JsonOpts), Encoding.UTF8, "application/json");

    public async Task<T> Get<T>(string url, string? token = null)
    {
        using var c = Create(token);
        var res = await c.GetAsync(url);
        var txt = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");

        if (typeof(T) == typeof(string))
            return (T)(object)txt;

        return JsonSerializer.Deserialize<T>(txt, JsonOpts)!;
    }

    public async Task<TOut> Post<TIn, TOut>(string url, TIn body, string? token = null)
    {
        using var c = Create(token);
        var res = await c.PostAsync(url, JsonBody(body));
        var txt = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");
        return JsonSerializer.Deserialize<TOut>(txt, JsonOpts)!;
    }

    public async Task Post<TIn>(string url, TIn body, string? token = null)
    {
        using var c = Create(token);
        var res = await c.PostAsync(url, JsonBody(body));
        var txt = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");
    }

    public async Task Delete(string url, string? token = null)
    {
        using var c = Create(token);
        var res = await c.DeleteAsync(url);
        var txt = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");
    }

    public async Task Put<TIn>(string url, TIn body, string? token = null)
    {
        using var c = Create(token);
        var res = await c.PutAsync(url, JsonBody(body));
        var txt = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");
    }

    public async Task DeleteWithBody<TIn>(string url, TIn body, string? token = null)
    {
        using var c = Create(token);

        var req = new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = JsonBody(body)
        };

        var res = await c.SendAsync(req);
        var txt = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"{(int)res.StatusCode} {res.ReasonPhrase}\n{txt}");
    }
}