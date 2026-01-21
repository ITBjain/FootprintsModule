using Microsoft.EntityFrameworkCore;
using PegasusFootprintsApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Get Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Safety Check
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string is missing in appsettings.json.");
}

// 2. Register MySQL Context
builder.Services.AddDbContext<FootprintsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 3. Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// FIX: Commented out because we are running on HTTP (localhost:5200)
// This stops the "Failed to determine https port" warning.
// app.UseHttpsRedirection(); 

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();