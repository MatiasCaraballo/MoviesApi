using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;

using System.Globalization;


var builder = WebApplication.CreateBuilder(args);


CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MovieAPI";
    config.Title        = "Movie API v1";
    config.Version      = "v1";
    
});

builder.Services.AddControllers();

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

//Routes
RouteGroupBuilder movieRoute = app.MapGroup("/movies");

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

app.Run();

            //Methods

//Index Movies
static async Task<IResult> GetAllMovies(CinemaDbContext db)
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
    Console.WriteLine(CultureInfo.CurrentCulture.Name); 
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

    //postedMovieDTO = new MovieDTO(moviePost);
    return TypedResults.Created($"/todoitems/{movieDTO}");  //movieDTO);*/
}
