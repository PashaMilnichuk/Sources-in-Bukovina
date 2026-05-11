using System.Text;
using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddTransient<CarpathianCrown.Api.Middleware.ExceptionHandlingMiddleware>();

var connectionString = builder.Configuration.GetValue<string>("DATABASE_URL")
                    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Database connection string is missing!");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString));

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<BookingDomainService>();

var analyticsUrl = builder.Configuration["Analytics:BaseUrl"];
if (!string.IsNullOrEmpty(analyticsUrl))
{
    builder.Services.AddHttpClient<AnalyticsClient>(client =>
    {
        client.BaseAddress = new Uri(analyticsUrl);
        client.Timeout = TimeSpan.FromSeconds(5);
    });
}

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT key is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("web", p =>
    {
        var origins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>() ?? Array.Empty<string>();
        p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введи JWT токен у форматі: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseCors("web");

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CarpathianCrown.Api.Middleware.ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedData.Ensure(db);
}

app.UseSerilogRequestLogging();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.UseRouting();
app.MapControllers();

app.Run();