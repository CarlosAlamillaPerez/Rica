using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace bepensa_web_common;

public static class SessionName
{
    public const string UsuarioActual = "usuario_actual";
}

public interface IAppSession
{
}

public class AppSession : IAppSession
{
    private readonly ISessionService _session;

    public AppSession(ISessionService session)
    {
        _session = session;
    }
}

#region Session Service
public class SessionEntry<T>
{
    private readonly string _key;
    private readonly ISessionService _service;

    public SessionEntry(string key, ISessionService service)
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
public interface ISessionService
{
    T Get<T>(string key);

    void Set<T>(string key, T value);

    void Delete(string key);

    void Clear();
}

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _http;

    public SessionService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public T Get<T>(string key)
    {
        var value = _http.HttpContext?.Session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public void Set<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        _http.HttpContext?.Session.SetString(key, json);
    }

    public void Delete(string key)
    {
        _http.HttpContext?.Session.Remove(key);
    }

    public void Clear()
    {
        _http.HttpContext.Session.Clear();
    }
}
#endregion