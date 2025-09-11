using Diamond.API.Data;
using Diamond.API.Repositories.ColorRepository;
using Diamond.API.Repositories.ShapeRepository;
using Diamond.API.Repositories.Shapes;
using Diamond.API.Services.Colors;
using Diamond.API.Services.Shapes;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));


// Register services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Dapper DB context
builder.Services.AddSingleton<DapperContext>();

// Repositories
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IShapeRepository, ShapeRepository>();

//Services
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IShapeService, ShapeService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
