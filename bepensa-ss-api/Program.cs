
using System.Text;
using bepensa_biz.Interfaces;
using bepensa_biz.Mapping;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_data.data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using bepensa_biz.Settings;
using bepensa_biz;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BepensaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(3),
                errorNumbersToAdd: null
            );
        });
});


// Posible argumento de referencia nulo
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
// Posible argumento de referencia nulo
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IEncryptor, EncryptorProxy>();

builder.Services.AddScoped<ISecurity, SecurityProxy>();

builder.Services.AddScoped<IBitacoraDeContrasenas, BitacoraDeContrasenasProxy>();
builder.Services.AddScoped<IBitacoraEnvioCorreo, BitacoraEnvioCorreoProxy>();
builder.Services.AddScoped<IDireccion, DireccionesProxy>();
builder.Services.AddScoped<ILlamada, LlamadasProxy>();
builder.Services.AddScoped<IInscripcion, InscripcionesProxy>();
builder.Services.AddScoped<IOperador, OperadoresProxy>();
builder.Services.AddScoped<IUsuario, UsuariosProxy>();

builder.Services.AddAutoMapper(typeof(DTOProfile));
builder.Services.AddAutoMapper(typeof(CRMProfile));
// builder.Services.AddScoped<IBitacoraDeUsuario, BitacoraDeUsuariosProxy>();
builder.Services.AddScoped<IEnviarCorreo, EnviarCorreoProxy>();
// builder.Services.AddScoped<IPeriodo, PeriodosProxy>();
// builder.Services.AddScoped<IUsuarios, UsuariosProxy>();
// builder.Services.AddScoped<INegocios, NegociosProxy>();
// builder.Services.AddScoped<IObjetivo, ObjetivosProxy>();
// builder.Services.AddScoped<IPremio, PremiosProxy>();
// builder.Services.AddScoped<IEstadoDeCuenta, EstadosDeCuentaProxy>();
// builder.Services.AddScoped<ICarrito, CarritoProxy>();
// builder.Services.AddScoped<IFuerzaDeVenta, FuerzasDeVentaProxy>();
// builder.Services.AddScoped<ISeccion, SeccionesProxy>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<GlobalSettings>(builder.Configuration.GetSection("Global"));
// builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("Url"));
// builder.Services.Configure<PremiosSettings>(builder.Configuration.GetSection("Premios"));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
