using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using RentACar.API.BackgroundServices;
using RentACar.API.Extensions;
using RentACar.API.Filters;
using RentACar.Business.DependencyResolvers.Autofac;
using RentACar.Business.ValidationRules.OfficeValidators;
using RentACar.DataAccess.Concrete.EntityFramework;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. IoC Container & Autofac Configuration ---
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(options =>
{
    options.RegisterModule(new AutofacBusinessModule());
});

// --- 2. Controllers & Filters ---
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilters>();
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

// --- 3. Background Services ---
builder.Services.AddHostedService<CarImageCleanupService>();

// --- 4. FluentValidation Setup ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<OfficeCreateDtoValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// --- 5. Security & Authentication ---
builder.Services.AddSecurityServices(builder.Configuration);

// --- 6. OpenAPI & Scalar (Swagger Alternative) Setup ---
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

// --- 7. AutoMapper Setup ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "Community-Student-License";
}, typeof(RentACar.Business.Profiles.CarProfile));

// --- 8. Database Context Setup ---
builder.Services.AddDbContext<RentACarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- 9. HTTP Request Pipeline (Middleware) ---

// Exception Middleware (Global Hata Yönetimi) - Her zaman en başta olmalı
app.UseMiddleware<RentACar.API.Middlewares.ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.MapScalarApiReference(options => options.Theme = ScalarTheme.Moon);
}

app.UseHttpsRedirection();

// Statik Dosyalar (wwwroot)
app.UseStaticFiles();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();