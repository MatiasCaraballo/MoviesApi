using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MoviesApp.Data;
using System.Globalization;
using System.Text;
using System.Security.Claims;
using MoviesApp.Helpers;
using FluentValidation;


//Use appSettings
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();  //important to use the jwToken

//Globalization
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


//DBContext
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<CinemaDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();


//Controllers
builder.Services.AddControllers();

//Helpers

builder.Services.AddScoped<AuthHelper>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IDirectorService,DirectorService>();

builder.Services.AddHttpContextAccessor();


//Token Jwtoken
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))
{

    throw new InvalidOperationException("JWT Key is missing in configuration.");
}


//Adds the Authentication and Authorization policy services
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = builder.Configuration["Jwt:RoleClaimType"],
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();


var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var roles = builder.Configuration["Jwt:RoleClaimType"]; // Roles

//Creates the Open Api Document
builder.Services.AddOpenApiDocument(config =>
{
        config.Title = "Movies Api";

        config.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
        {
            Type = NSwag.OpenApiSecuritySchemeType.Http,
            Scheme = "bearer",
            In = NSwag.OpenApiSecurityApiKeyLocation.Header,
            Description = "Include your JWT token"
        });

        config.OperationProcessors.Add(
            new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


//Runs Swagger interface
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseOpenApi();

    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "MovieAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";

    }
    );
}

app.MapControllers();
//Routes
RouteGroupBuilder movieRoute = app.MapGroup("/movies");
RouteGroupBuilder directorRoute = app.MapGroup("/directors");
RouteGroupBuilder directorsMovieRoute = app.MapGroup("/directors-movies");

app.Run();


