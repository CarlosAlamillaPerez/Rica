using bepensa_biz.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using bepensa_biz.Mapping;
using bepensa_ss_web.Configuratioin;
using bepensa_ss_web.Areas.FuerzaVenta.Filters;
using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;

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
    options.Cookie.Name = "LMS.BepensaWeb";

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
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/";
    options.ReturnUrlParameter = "authUrl";
    options.Cookie.Name = "LMS.BepensaWebAuth";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;

    options.Events.OnValidatePrincipal = async context =>
    {
        var isLoggued = context.HttpContext.Session.GetString("usuario_actual") != null;

        var isLogguedFDV = context.HttpContext.Session.GetString("fdv_actual") != null;

        if (!isLoggued && !isLogguedFDV)
        {
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.RejectPrincipal();
        }
    };
    options.Events.OnRedirectToLogin = context =>
    {
        string redirectUri = context.RedirectUri;

        var isLoggued = context.HttpContext.Session.GetString("usuario_actual") != null;

        var isLogguedFDV = context.HttpContext.Session.GetString("fdv_actual") != null;

        //if ((isLoggued && !isLogguedFDV) || (isLogguedFDV && !isLoggued) || (isLoggued && isLogguedFDV))
        if (isLoggued || isLogguedFDV)
        {
            context.HttpContext.Session.Clear();
        }

        context.Response.Redirect(redirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAutoMapper(typeof(DTOProfile));

builder.Services.AppDatabase(builder.Configuration);

builder.Services.AppServices();
builder.Services.AddScoped<IEncuesta, EncuestaProxy>();

//------------------------------------- DinkToPdf -------------------------------------
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
//------------------------------------ DinkToPdf End ------------------------------------

builder.Services.AppSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews()
.AddSessionStateTempDataProvider()
.AddRazorRuntimeCompilation();

builder.Services.AddMemoryCache();

//------------------------------------- Logger -------------------------------------
builder.Host.UseSerilog((context, services, configuration) =>
{
    var dbLoggerString = context.Configuration.GetConnectionString("DBLoggerContext");

    Console.WriteLine(builder.Configuration.GetConnectionString("DBLoggerContext"));

    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
        .Enrich.WithExceptionDetails()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {Message}{NewLine}{Exception}{Properties:j}");

    if (!string.IsNullOrEmpty(dbLoggerString))
    {
        configuration.WriteTo.MSSqlServer(
            connectionString: dbLoggerString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false
            },
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
        );
    }
});
//------------------------------------- Logger End -------------------------------------

var app = builder.Build();
//------------------------------------- DinkToPdf -------------------------------------
// Cargar la librería nativa para DinkToPdf (solo en Windows)
var context = new CustomAssemblyLoadContext();
var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox", "libwkhtmltox.dll");

if (!File.Exists(dllPath))
{
    throw new FileNotFoundException("No se encontró libwkhtmltox.dll", dllPath);
}

context.LoadUnmanagedLibrary(dllPath);
//------------------------------------ DinkToPdf End ------------------------------------

var isDev = builder.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (builder.Configuration.GetValue<bool>("Global:Produccion"))
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

if (!isDev)
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
    ctx.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

    if (ctx.Request.Path.StartsWithSegments("/bepensa-app"))
    {
        ctx.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate");
        ctx.Response.Headers.Append("Pragma", "no-cache");
        ctx.Response.Headers.Append("Expires", "0");

        await next();

        return;
    }

    ctx.Response.OnStarting(() =>
    {
        ctx.Response.Headers.Remove("X-Powered-By");
        ctx.Response.Headers.Remove("Server");
        return Task.CompletedTask;
    });

    var hash = new Hash(Guid.NewGuid().ToString());
    
    var nonce = hash.ToSha256();

    var mySite = builder.Configuration.GetValue<bool>("Global:Produccion") ?
        builder.Configuration.GetValue<string>("Global:Url") :
        builder.Configuration.GetValue<string>("Global:UrlLocal") ?? string.Empty;

    var addSitesImgUrl = builder.Configuration.GetValue<string>("Global:ImgSrc") ?? "";

    var urlIframe = builder.Configuration.GetValue<string>("Global:UrlIframe") ?? string.Empty;

    var openPayPolicy = builder.Configuration.GetValue<bool>("Global:Produccion")
        ? builder.Configuration.GetValue<string>("OpenPay:UrlPolicyProd") 
        : builder.Configuration.GetValue<string>("OpenPay:UrlPolicyQA") ?? string.Empty;

    var basePolicy = "base-uri 'self'; ";
    var scriptPolicy = $"script-src {mySite} 'nonce-{nonce}' " +
                    "https://fonts.googleapis.com/ https://cdnjs.cloudflare.com/ " +
                    "https://cdn.jsdelivr.net/ https://db.onlinewebfonts.com/ " +
                   "https://cdnjs.cloudflare.com/ https://cdn.jsdelivr.net/ " +
                   $"{openPayPolicy} " +
                   "https://cdn.siftscience.com " +
                   "https://*.google-analytics.com https://*.googletagmanager.com " +
                   "'unsafe-eval' 'self'; ";

    var childPolicy = $"child-src {mySite} 'self' {openPayPolicy};";

    var objectPolicy = $"object-src {mySite} 'self' blob:; ";
    var fontPolicy = "font-src https://fonts.googleapis.com https://fonts.gstatic.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://db.onlinewebfonts.com 'self' data:;";
    var imgPolicy = $"img-src 'self' {mySite} {addSitesImgUrl} https://hexagon-analytics.com https://*.google-analytics.com data:;";
    var defaultPolicy = "default-src 'self';";
    var stylePolicy = "style-src https://fonts.googleapis.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://db.onlinewebfonts.com https://*.google-analytics.com https://*.googletagmanager.com 'self' 'unsafe-inline';";
    var iframePolicy = $"frame-ancestors 'self' {mySite} {urlIframe} {openPayPolicy} https://*.google-analytics.com https://*.googletagmanager.com;";

    var connectPolicy = isDev
        ? $"connect-src 'self' ws: wss: http://localhost:* https://localhost:* {addSitesImgUrl} {addSitesImgUrl} {openPayPolicy} https://*.google-analytics.com https//*.analytics.google.com https://*.googletagmanager.com;"
        : $"connect-src 'self' {addSitesImgUrl} {addSitesImgUrl} {openPayPolicy} https://*.google-analytics.com https//*.analytics.google.com https://*.googletagmanager.com;";

    var csp = string.Join(" ",
        defaultPolicy,
        basePolicy,
        stylePolicy,
        childPolicy,
        scriptPolicy,
        fontPolicy,
        objectPolicy,
        imgPolicy,
        iframePolicy,
        connectPolicy
    );

    ctx.Response.Headers.Append("Content-Security-Policy", csp);

    if (builder.Configuration.GetValue<bool>("Global:CSPReport"))
    {
        ctx.Response.Headers.Append("Content-Security-Policy-Report-Only", "default-src 'self'; report-uri /csp-report");
    }

    ctx.Items["ScriptNonce"] = nonce;

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