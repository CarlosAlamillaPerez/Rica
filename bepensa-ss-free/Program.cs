using bepensa_biz.Interfaces;
using bepensa_biz.Mapping;
using bepensa_biz.Proxies;
using bepensa_data.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<BepensaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext"), sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null
        );
    });
});

builder.Services.AddAutoMapper(typeof(DTOProfile));

builder.Services.AddScoped<IObjetivo, ObjetivosProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Socio}/{action=Index}/{id?}");

app.Run();
