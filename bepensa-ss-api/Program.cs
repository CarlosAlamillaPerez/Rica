using Microsoft.AspNetCore.Mvc;
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
using bepensa_biz.Settings;
using bepensa_biz;
using bepensa_data.logger.data;
using Microsoft.AspNetCore.Authentication;
using bepensa_ss_api.Configuratioin;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using bepensa_models.Logger;
using System.Threading.Channels;


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

builder.Services.AddDbContext<BepensaLoggerContext>(options =>
{
    var pruba = builder.Configuration.GetConnectionString("DBLoggerContext");

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DBLoggerContext"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(3),
                errorNumbersToAdd: null
            );
        }
    );
}, ServiceLifetime.Scoped);


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
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.ApiKeySchemeName, options => { }
    );

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
builder.Services.AddScoped<IApp, AppProxy>();
builder.Services.AddScoped<IEdoCta, EdoCtaProxy>();

builder.Services.AddAutoMapper(typeof(DTOProfile));
builder.Services.AddAutoMapper(typeof(CRMProfile));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// builder.Services.AddScoped<IBitacoraDeUsuario, BitacoraDeUsuariosProxy>();
builder.Services.AddScoped<IEnviarCorreo, EnviarCorreoProxy>();
builder.Services.AddScoped<IPeriodo, PeriodosProxy>();
builder.Services.AddScoped<IUsuario, UsuariosProxy>();
// builder.Services.AddScoped<INegocios, NegociosProxy>();
builder.Services.AddScoped<IObjetivo, ObjetivosProxy>();
builder.Services.AddScoped<IPremio, PremiosProxy>();
builder.Services.AddScoped<ICarrito, CarritoProxy>();
builder.Services.AddScoped<IFuerzaVenta, FuerzaVentaProxy>();
// builder.Services.AddScoped<ISeccion, SeccionesProxy>();
builder.Services.AddScoped<IAppEmail, EmailProxy>();
builder.Services.AddScoped<IEncuesta, EncuestaProxy>();
builder.Services.AddScoped<IApi, ApiProxy>();
builder.Services.AddScoped<IBitacora, BitacoraProxy>();
builder.Services.AddScoped<ILoggerContext, LoggerProxy>();
builder.Services.AddScoped<IOperacion, OperacionesProxy>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<GlobalSettings>(builder.Configuration.GetSection("Global"));
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.Configure<PremiosSettings>(builder.Configuration.GetSection("Premios"));
builder.Services.Configure<ApiRMSSettings>(builder.Configuration.GetSection("ApiRms"));
builder.Services.Configure<ApiCPDSettings>(builder.Configuration.GetSection("ApiCPD"));

builder.Services.AddDistributedMemoryCache();

//------------------------------------- Logger -------------------------------------
builder.Host.UseSerilog((context, services, configuration) =>
{
    var dbLoggerString = context.Configuration.GetConnectionString("DBLoggerContext");

    Console.WriteLine(builder.Configuration.GetConnectionString("DBLoggerContext"));

    configuration
        .MinimumLevel.Information() // Nivel mínimo a registrar
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Se controla registro de error originarios de Microsoft
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) // Se controla registro de error originarioa de System
        .Enrich.WithExceptionDetails() // Agrega detalle completo al log
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {Message}{NewLine}{Exception}{Properties:j}");

    if (!string.IsNullOrEmpty(dbLoggerString))
    {
        configuration.WriteTo.MSSqlServer(
            connectionString: dbLoggerString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs", // Nombre de la tabla
                AutoCreateSqlTable = false // Evita que se cree la tabla automáticamente en caso de no existir.
            },
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
        );
    }
});
//------------------------------------- Logger End -------------------------------------

//------------------------------------- Logger ExternalApi -------------------------------------
builder.Services.AddSingleton(Channel.CreateUnbounded<ExternalApiLogger>());

builder.Services.AddHostedService<ExternalApiLogBackgroundService>();
//------------------------------------- Logger ExternalApi -------------------------------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bepensa Web API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "LMS",
            Url = new Uri("https://www.lms-la.com/")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,

        Description = "Ingrese 'Bearer' [espacio] y luego su token JWT"
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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
// Configurar HSTS

builder.Services.AddHsts(options =>

{

    options.MaxAge = TimeSpan.FromDays(365); // Duración del HSTS, por ejemplo, 1 año

    options.IncludeSubDomains = true;        // Aplica HSTS a todos los subdominios

    options.Preload = true;                  // Permite el preload en la lista de HSTS

});


var app = builder.Build();

app.UseHsts();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
