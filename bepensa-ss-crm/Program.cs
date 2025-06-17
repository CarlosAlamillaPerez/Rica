using bepensa_biz.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using bepensa_biz.Mapping;
using bepensa_ss_crm.Configuratioin;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

const string CultureDefault = "es-MX";

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo(CultureDefault)
        {
            DateTimeFormat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "dd/MM/yyyy hh:mm:ss tt",
                DateSeparator = "/"
            }
        },
        new CultureInfo("es")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: CultureDefault, uiCulture: CultureDefault);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
    {
        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(CultureDefault));
    }));
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = HttpOnlyPolicy.Always;
});

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

builder.Services.AddDistributedMemoryCache(); // Almacena datos temporales.

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "BepensaMX.CRM.LMS";

    options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<double>("Global:Sesion:Expiracion"));
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/";
    options.AccessDeniedPath = "/";
    options.ReturnUrlParameter = "authUrl";
    options.Cookie.Name = "BepensaMX.Auth.LMS";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;

    options.Events.OnValidatePrincipal = async context =>
    {
        var isLoggued = context.HttpContext.Session.GetString("operador_actual") != null;
        if (!isLoggued)
        {
            await context.HttpContext.SignOutAsync();
            context.RejectPrincipal();
        }
    };
    options.Events.OnRedirectToLogin = context =>
    {
        var isLoggued = context.HttpContext.Session.GetString("operador_actual") != null;
        if (isLoggued)
        {
            context.HttpContext.Session.Clear();
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAutoMapper(typeof(DTOProfile));
builder.Services.AddAutoMapper(typeof(CRMProfile));

// Inicializamos configuraci�n de la Base de Datos
builder.Services.AppDatabase(builder.Configuration);

// Servicios de Interfaz/Proxy's (Dentro del m�todo realiza las inyecciones de dependecias que requiera la app)
builder.Services.AppServices();
builder.Services.AppServicesCRM();

// Inyecci�n de configuraciones establecidad en el appsetting.json
builder.Services.AppSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider()
    .AddRazorRuntimeCompilation();

builder.Services.AddMemoryCache();

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("Global:Produccion"))
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (ctx, next) =>
{
    var hash = new Hash(Guid.NewGuid().ToString());

    var sitesImgUrl = builder.Configuration.GetValue<bool>("Global:Produccion") ?
        builder.Configuration.GetValue<string>("Global:Url") :
        "http://localhost:44301/ http://localhost:5167 https://localhost:7199";

    var addSitesImgUrl = builder.Configuration.GetValue<string>("Global:ImgSrc");

    var defaultPolicy = "default-src 'self';";
    var basePolicy = "base-uri 'self';";
    var stylePolicy = "style-src https://fonts.googleapis.com/ 'self' 'unsafe-inline';";
    var scriptPolicy = $"script-src {sitesImgUrl} 'nonce-{hash.ToSha256()}' https://cdnjs.cloudflare.com/ 'unsafe-eval' 'self';";
    var childPolicy = $"child-src {sitesImgUrl} 'self';";
    var objectPolicy = $"object-src {sitesImgUrl} 'self' blob:;";
    var fontPolicy = "font-src https://fonts.googleapis.com/ https://fonts.gstatic.com/ 'self' data:;";
    var imgPolicy = $"img-src 'self' {sitesImgUrl} {addSitesImgUrl} data: https://dummyimage.com/;";
    var iframePolicy = $"frame-ancestors 'self' {sitesImgUrl} {addSitesImgUrl};";
    var connectPolicy = $"connect-src 'self' {addSitesImgUrl} {sitesImgUrl} ws: wss:;";

    ctx.Response.Headers.Append("Content-Security-Policy", $"{defaultPolicy}{basePolicy}{stylePolicy}{childPolicy}{scriptPolicy}{fontPolicy}{objectPolicy}{imgPolicy}{iframePolicy}{connectPolicy}");

    ctx.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    ctx.Items["ScriptNonce"] = hash.ToSha256();
    await next();
});

app.MapControllerRoute(
      name: "defaultarea",
      pattern: "{area=Autenticacion}/{controller=Cuentas}/{action=Login}/{id?}");

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Cuentas}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
