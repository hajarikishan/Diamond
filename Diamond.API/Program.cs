using Diamond.API.Data;
using Diamond.API.Repositories;
using Diamond.API.Repositories.Clarity;
using Diamond.API.Repositories.ColorRepository;
using Diamond.API.Repositories.Cut;
using Diamond.API.Repositories.Polish;
using Diamond.API.Repositories.Purity;
using Diamond.API.Repositories.ShapeRepository;
using Diamond.API.Repositories.Shapes;
using Diamond.API.Services.Clarity;
using Diamond.API.Services.Colors;
using Diamond.API.Services.Cut;
using Diamond.API.Services.Email;
using Diamond.API.Services.Polish;
using Diamond.API.Services.Purity;
using Diamond.API.Services.Shapes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

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
builder.Services.AddScoped<UserRepository>();

// Services
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IShapeService, ShapeService>();
builder.Services.AddScoped<IClarityService, ClarityService>();
builder.Services.AddScoped<ICutService, CutService>();
builder.Services.AddScoped<IPurityService, PurityService>();
builder.Services.AddScoped<IPolishService, PolishService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// ✅ JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
