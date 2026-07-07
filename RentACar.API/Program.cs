using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Contexts;
using RentACar.Business.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Business katmanındaki gizli çantamızı buraya tek satırla çağırıyoruz
builder.Services.AddBusinessServices();

// SWAGGER KAYITLARI (Swashbuckle.AspNetCore paketi gerektirir)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
{
    // AutoMapper v15+ Lisans Yapılandırması
    cfg.LicenseKey = "Community-Student-License";
}, typeof(RentACar.Business.Profiles.CarProfile));

// PostgreSQL ve DbContext Kaydı
builder.Services.AddDbContext<RentACarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// SWAGGER ARAYÜZÜNÜ TETİKLEME
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();