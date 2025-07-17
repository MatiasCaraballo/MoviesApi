using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MoviesApp.Data;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();  //importante to use the jwToken

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

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MovieAPI";
    config.Title        = "Movie API v1";
    config.Version      = "v1";
    
});

//Controllers
builder.Services.AddControllers();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMovieService, MovieService>();


//Token Jwtoken
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))
{

    throw new InvalidOperationException("JWT Key is missing in configuration.");
}

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});


var app = builder.Build();

//Runs Swagger
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
/*
movieRoute.MapGet("/", GetAllMovies).WithName("GetAllMovies")
         .WithTags("Movies")
         .WithDescription("get movies list")
         .Produces<MovieDTO>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound);

movieRoute.MapGet("/{id}", GetMovie).WithName("GetMovie")
         .WithName("GetMovie").WithTags("Movies")
         .WithDescription("Gets a Movie By Id")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound) 
         .ProducesProblem(StatusCodes.Status500InternalServerError);

movieRoute.MapPost("/", PostMovie)
         .WithName("Post Movie")
         .WithTags("Movies")
         .WithDescription("Creates the movie")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound)
         .ProducesProblem(StatusCodes.Status500InternalServerError);


movieRoute.MapPut("/{id}", UpdateMovie)
         .WithName("UpdateMovie")
         .WithTags("Movies")
         .WithDescription("Updates a movie")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound)
         .ProducesProblem(StatusCodes.Status500InternalServerError);

movieRoute.MapDelete("/{id}", DeleteMovie)
         .WithName("Delete Movie")
         .WithTags("Movies")
         .WithDescription("Deletes an Movie")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound)
         .ProducesProblem(StatusCodes.Status500InternalServerError);
*/
RouteGroupBuilder directorRoute = app.MapGroup("/directors");

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





app.Run();

            //Methods

//Index Movies
/*static async Task<IResult> GetAllMovies(CinemaDbContext db)
{
    return TypedResults.Ok(await db.Movies.Select(x => new MovieDTO(x)).ToArrayAsync());
}

//Show movie by Id
static async Task<IResult> GetMovie(int id, CinemaDbContext db)
{
    return await db.Movies.FindAsync(id)
        is Movie movie
            ? TypedResults.Ok(new MovieDTO(movie))
            : TypedResults.NotFound();
}

//Creates an movie
static async Task<IResult> PostMovie(MovieDTO movieDTO, CinemaDbContext db)
{
    var movie = new Movie
    {
        Name           = movieDTO.Name,
        ReleaseDate    = movieDTO.ReleaseDate,
        Classification = movieDTO.Classification,
        ImdbRating     = movieDTO.ImdbRating,
        CreatedAt      = DateTime.Now
    };

    db.Movies.Add(movie);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/movies/{movieDTO}");  //movieDTO);*/
//}
/*
static async Task<IResult> UpdateMovie(int id, MovieDTO movieDTO, CinemaDbContext db)
{
    var movie = await db.Movies.FindAsync(id);

    if (movie is null) return TypedResults.NotFound();

    movie.Name = movieDTO.Name;
    movie.Classification = movieDTO.Classification;
    movie.ReleaseDate = movieDTO.ReleaseDate;
    movie.ImdbRating = movieDTO.ImdbRating;
    
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

//Deletes the movie
static async Task<IResult> DeleteMovie(int id, CinemaDbContext db)
{
    if (await db.Movies.FindAsync(id) is Movie movie)
    {
        db.Movies.Remove(movie);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}

*/
///Index Directors
static async Task<IResult> GetAllDirectors(CinemaDbContext db)
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

    
}
   
