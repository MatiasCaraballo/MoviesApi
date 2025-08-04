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
        RoleClaimType = ClaimTypes.Role // Roles
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
            Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
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

/*
directorRoute.MapGet("/",GetAllDirectors).WithName("GetAllDirectors")
         .WithTags("Directors")
         .WithDescription("get directors list")
         .Produces<DirectorDTO>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound);

directorRoute.MapGet("/{id}", GetDirector).WithName("GetDirector")
         .WithName("GetDirector").WithTags("Directors")
         .WithDescription("Gets a Director By Id")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound) 
         .ProducesProblem(StatusCodes.Status500InternalServerError);

directorRoute.MapPost("/", PostDirector)
         .WithName("Post Director")
         .WithTags("Directors")
         .WithDescription("Creates the director")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound)
         .ProducesProblem(StatusCodes.Status500InternalServerError);

RouteGroupBuilder moviesDirectorsRoute = app.MapGroup("/movie-directors");

// Post an MoviesDirectors relationship
moviesDirectorsRoute.MapPost("/",PostMovieDirector)
        .WithName("Post MovieDirector")
        .WithTags("MoviesDirectors")
        .WithDescription("Post MovieDirector Relationship")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

//Get all director by Movie
moviesDirectorsRoute.MapGet("/movie{movieId}-directors", GetDirectorsByMovie).WithName("GetDirectorsByMovie")
        .WithTags("MoviesDirectors")
        .WithDescription("Get all director by an movieId")
        .WithDescription("Creates the director")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);



*/


            //Methods

///Index Directors
/*static async Task<IResult> GetAllDirectors(CinemaDbContext db)
{
    return TypedResults.Ok(await db.Directors.Select(x => new DirectorDTO(x)).ToArrayAsync());
}

//Show movie by Id
static async Task<IResult> GetDirector(int id, CinemaDbContext db)
{
    return await db.Directors.FindAsync(id)
        is Director director
            ? TypedResults.Ok(new DirectorDTO(director))
            : TypedResults.NotFound();
}

//Creates an director
static async Task<IResult> PostDirector(DirectorDTO directorDTO, CinemaDbContext db)
{
    var director = new Director
    {
        Name = directorDTO.Name,
        Surname = directorDTO.Surname,
        Country = directorDTO.Country,
        BirthDate = directorDTO.BirthDate,
        CreatedAt = DateTime.Now,
    };

    db.Directors.Add(director);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/directors/{directorDTO}");
}

static async Task<IResult> GetDirectorsByMovie(int movieId,CinemaDbContext db)
{
    var movie = await db.Movies
                    .Include(m => m.Directors)
                    .FirstOrDefaultAsync(m => m.MovieId == movieId);

    var result = await db.Movies.SelectMany(
                        m => m.Directors,
                        (m, d) => new
                        {
                            MovieId = m.MovieId,
                            Movie = m.Name,
                            DirectorId = d.DirectorId,
                            Director = d.Name

                        }
                ).ToListAsync();           
    return TypedResults.Ok(result);            

}

static async Task <IResult> PostMovieDirector(int movieId,int directorId,CinemaDbContext db)
{
    var movie = await db.Movies
                     .Include(m => m.Directors)
                     .FirstOrDefaultAsync(m => m.MovieId == movieId);

    if (movie == null)
        return Results.NotFound($"Movie {movieId} not found.");

    var director = await db.Directors.FindAsync(directorId);

    if (director == null)
        return Results.NotFound($"Director {directorId} not found.");

    if (movie.Directors.Any(d => d.DirectorId == directorId))
        return Results.Conflict("The director its asigned to the movie.");

    movie.Directors.Add(director);

    await db.SaveChangesAsync();

    return Results.Created($"/movie/{movieId}/director/{directorId}", null);

    
}*/
   
