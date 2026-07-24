using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using RentACar.API.Extensions;
using RentACar.Business.Extensions;
using RentACar.DataAccess.Concrete.EntityFramework;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Business katmanındaki gizli çantamızı buraya tek satırla çağırıyoruz
builder.Services.AddBusinessServices();

// Kendi yazdığımız güvenlik ve JWT ayarlarını içeri alıyoruz
builder.Services.AddSecurityServices(builder.Configuration);

// .NET 10 Modern OpenAPI / Swagger Kaydı
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Bearer token giriniz (Bearer kelimesi olmadan)"
        };

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});


builder.Services.AddAutoMapper(cfg =>
{
    // AutoMapper v15+ Lisans Yapılandırması
    cfg.LicenseKey = "Community-Student-License";
}, typeof(RentACar.Business.Profiles.CarProfile));

// PostgreSQL ve DbContext Kaydı
builder.Services.AddDbContext<RentACarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // .NET 10 Endpoint'ini açık

    // Scalar Arayüzünü .NET 10 Standartlarında Tetikliyoruz
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
// Kendi yazdığımız hata yakalayıcı kalkanı boru hattının en başına takıyoruz
app.UseMiddleware<RentACar.API.Middlewares.ExceptionMiddleware>();

// Authentication - Kimlik Doğrulama
app.UseAuthentication();
// Authorization - Yetkilendirme
app.UseAuthorization();
app.MapControllers();
app.Run();