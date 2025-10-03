using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MoviesApp.Data;
using System.Globalization;
using System.Text;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);



//Globalization
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


//DBContext
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // revisar

builder.Services.AddEndpointsApiExplorer();


//Controllers
builder.Services.AddControllers();

//Services

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IDirectorService,DirectorService>();


builder.Services.AddHttpContextAccessor();


//Adds the Authentication and Authorization policy services
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey)) { throw new InvalidOperationException("No JWT:Key was specified in the configuration"); }

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrEmpty(jwtIssuer)) { throw new InvalidOperationException("No JWT:issuer was specified in the configuration"); }

var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrEmpty(jwtAudience)){throw new InvalidOperationException("No JWT:audience was specified in the configuration");}


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
        NameClaimType = ClaimTypes.Name
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


//Creates the Open Api Document
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Movies Api";

    config.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        BearerFormat = "JWT",
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


