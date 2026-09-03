using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RentACar.MVC.Handlers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Restoran (MVC) fedaisine kural kitabını veriyoruz!
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            // Kasadaki TokenOptions altındaki Issuer'ı getir:
            ValidIssuer = builder.Configuration["TokenOptions:Issuer"],

            // Kasadaki TokenOptions altındaki Audience'ı getir:
            ValidAudience = builder.Configuration["TokenOptions:Audience"],

            // Kasadaki SecurityKey'i getir ve byte dizisine (demire) çevirip mühür yap:
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"]!))
        };

        // SİHİRLİ DOKUNUŞ: Fedaiye "VIP Kartı cebinden (Cookie) çıkarıp oku!" diyoruz.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Tarayıcıdaki cüzdanı aç, AccessToken var mı bak
                var token = context.Request.Cookies["AccessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    // Varsa bunu fedaiye teslim et
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

// 1. Maymuncuk Anahtarı (Cüzdana erişim için)
builder.Services.AddHttpContextAccessor();

// 2. Gümrük Memurunu işe al (Her istekte yeni bir memur - Transient)
builder.Services.AddTransient<AuthTokenHandler>();

// 3. Kuryeyi yarat VE YAKASINA MEMURU TAK! (İşte kritik nokta burası)
builder.Services.AddHttpClient("RentACarApi", client =>
{
    // Ayar defterindeki o BaseUrl adresini okuyup kuryenin çantasına sabitliyoruz
    // Ayar defterindeki o BaseUrl adresini okuyup kuryenin çantasına sabitliyoruz
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
})
.AddHttpMessageHandler<AuthTokenHandler>(); // <-- BUM! Kurye artık çıkarken memura uğrayacak.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
