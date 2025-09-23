using Diamond.API.Data;
using Diamond.API.Repositories.Clarity;
using Diamond.API.Repositories.ColorRepository;
using Diamond.API.Repositories.Cut;
using Diamond.API.Repositories.Dashboard;
using Diamond.API.Repositories.Polish;
using Diamond.API.Repositories.Purity;
using Diamond.API.Repositories.ShapeRepository;
using Diamond.API.Repositories.Shapes;
using Diamond.API.Repositories;
using Diamond.API.Services;
using Diamond.API.Services.Clarity;
using Diamond.API.Services.Colors;
using Diamond.API.Services.Cut;
using Diamond.API.Services.Polish;
using Diamond.API.Services.Purity;
using Diamond.API.Services.Shapes;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Register controllers + OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Dapper DB context
builder.Services.AddSingleton<DapperContext>();

// Repositories
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IShapeRepository, ShapeRepository>();
builder.Services.AddScoped<IClarityRepository, ClarityRepository>();
builder.Services.AddScoped<ICutRepository, CutRepository>();
builder.Services.AddScoped<IPurityRepository, PurityRepository>();
builder.Services.AddScoped<IPolishRepository, PolishRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<UserRepository>();

// Services
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IShapeService, ShapeService>();
builder.Services.AddScoped<IClarityService, ClarityService>();
builder.Services.AddScoped<ICutService, CutService>();
builder.Services.AddScoped<IPurityService, PurityService>();
builder.Services.AddScoped<IPolishService, PolishService>();
builder.Services.AddScoped<AuthService>();

// JWT auth
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // set true in prod
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
