using Microsoft.EntityFrameworkCore;
using HealthcareApp.Data;
using HealthcareApp.Repositories;
using HealthcareApp.Services;
using HealthcareApp.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Redis Cache
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379",
        true);
    return ConnectionMultiplexer.Connect(configuration);
});

// Register services
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ICaregiverRepository, CaregiverRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ICaregiverService, CaregiverService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

// Custom middleware
app.UseMiddleware<AuditMiddleware>();

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// Seed sample data in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await HealthcareApp.Data.DataSeeder.SeedAsync(context);
}

app.Run();
