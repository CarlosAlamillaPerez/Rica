using bepensa_biz;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Mapping;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_biz.Settings;
using bepensa_data.data;
using Business.BepensaWhatsapp.Extensiones;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.biz.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.biz.{envName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<AppConfigSettings>(builder.Configuration.GetSection("AppConfig"));

var provider = builder.Services.BuildServiceProvider();
var Configuration = provider.GetRequiredService<IConfiguration>();

// Contextos y cadenas de conexión
builder.Services.AddDbContext<BepensaRD_Context>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString(name: "ConnectionBepensaWaRD")).EnableSensitiveDataLogging();
    options.UseLazyLoadingProxies();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetValue<String>("AppSettings:TokeyKey"))), //configuration["AppSettings:TokeyKey"]
        ClockSkew = TimeSpan.Zero
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Manual Tecnico API Bepensa RD Whatsapp",
        Description = "Proveera el intercambio de datos entre aplicaciones, satisfaciendo las necesidades del cliente",
        TermsOfService = new Uri("https://www.lms-la.com/"),
        Contact = new OpenApiContact
        {
            Name = "Ricardo Hernández Martínez",
            Email = "ricardo.hernandez@mx.lms-la.com",
            Url = new Uri("https://www.lms-la.com/")
        },
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddScoped<ISecurity, SecurityProxy>();

builder.Services.AddScoped<IConsultasClienteProxy, ConsultaClienteRDProxy>();
builder.Services.AddScoped<IConsultasCuentaRDProxy, ConsultaCuentaRDProxy>();
builder.Services.AddScoped<IConsultasSugerenciaProxy, ConsultaSugerenciaRDProxy>();
builder.Services.AddScoped<IConsultasPremiosRDProxy, ConsultaPremioRDProxy>();
builder.Services.AddScoped<IConsultasProgramasRDProxy, ConsultaProgramaRDProxy>();
builder.Services.AddScoped<IConsultasPromocionesRDProxy, ConsultaPromocionesRDProxy>();
builder.Services.AddScoped<IConsultasAvanceRDProxy, ConsultaAvanceRDProxy>();
builder.Services.AddScoped<IUsuarioRD, UsuariosRDPRoxy>();
builder.Services.AddScoped<IAppEmailRD, EmailRDProxy>();

builder.Services.AddAutoMapper(typeof(DTOProfile));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.RegisterProxies();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<RawRequest>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
