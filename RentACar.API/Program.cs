using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(cfg =>
{
    // AutoMapper v15+ Lisans Yapılandırması
    // automapper.io'dan ücretsiz öğrenci/community key'ini alıp buraya yazabilirsin.
    // Şimdilik boş bırakıp veya "Community" yazıp geçsek de audit (denetim) amaçlı olduğu için çalışmaya devam edecektir.
    cfg.LicenseKey = "Community-Student-License";

}, typeof(RentACar.Business.Profiles.CarProfile));

// PostgreSQL ve DbContext Kaydı (Dependency Injection)
builder.Services.AddDbContext<RentACarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
