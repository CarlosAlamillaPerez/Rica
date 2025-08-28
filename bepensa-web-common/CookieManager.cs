using bepensa_web_common.Data;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace bepensa_web_common;

public static class CookieNames
{
}

public interface IAppCookies
{
    public CookieEntry<PremioCookie> Premio { get; }
}

public class AppCookies : IAppCookies
{
    private readonly ICookieService _cookieService;

    public AppCookies(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }

    public CookieEntry<PremioCookie> Premio => new(nameof(Premio), _cookieService);
}

#region Cookie Service
public class CookieEntry<T>
{
    private readonly string _key;

    private readonly ICookieService _service;

    public CookieEntry(string key, ICookieService service)
    {
        _key = key;
        _service = service;
    }

    public T Value
    {
        get => _service.Get<T>(_key);
        set => _service.Set(_key, value);
    }

    public void Delete() => _service.Delete(_key);
}

public interface ICookieService
{
    T Get<T>(string key);

    void Set<T>(string key, T value, int days = 1);

    void Delete(string key);
}

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _http;

    public CookieService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public T Get<T>(string key)
    {
        var cookie = _http.HttpContext?.Request.Cookies[key];
        return string.IsNullOrWhiteSpace(cookie) ? default : JsonSerializer.Deserialize<T>(cookie);
    }

    public void Set<T>(string key, T value, int days = 1)
    {
        var json = JsonSerializer.Serialize(value);
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(days),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        _http.HttpContext?.Response.Cookies.Append(key, json, options);
    }

    public void Delete(string key)
    {
        _http.HttpContext?.Response.Cookies.Delete(key);
    }
}
#endregion