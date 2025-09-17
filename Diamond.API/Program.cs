using Diamond.API.Data;
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
using Diamond.API.Services.Polish;
using Diamond.API.Services.Purity;
using Diamond.API.Services.Shapes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Diamond.API.Services.Users;
using Diamond.API.Repositories.Users;

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
builder.Services.AddScoped<IClarityRepository, ClarityRepository>();
builder.Services.AddScoped<ICutRepository, CutRepository>();
builder.Services.AddScoped<IPurityRepository, PurityRepository>();
builder.Services.AddScoped<IPolishRepository, PolishRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IShapeService, ShapeService>();
builder.Services.AddScoped<IClarityService, ClarityService>();
builder.Services.AddScoped<ICutService, CutService>();
builder.Services.AddScoped<IPurityService, PurityService>();
builder.Services.AddScoped<IPolishService, PolishService>();
builder.Services.AddScoped<IUserService, UserService>();

// JWT auth
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});

// CORS: allow Blazor UI origin
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowBlazorDev", p =>
    {
        p.WithOrigins("http://localhost:5017")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowBlazorDev");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.Run();
