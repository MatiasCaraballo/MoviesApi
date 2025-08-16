using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]

public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]


    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
    {
        try
        {
            var movies = await _movieService.GetAllMovies();
            return Ok(movies);

        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");

        }
    }

    [HttpGet("/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    
    public async Task<ActionResult<MovieDTO>> GetMovie(int id)
    {
        try
        {
            var movie = await _movieService.GetMovie(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDTO)
    {
        try
        {
            var movie = await _movieService.PostMovie(movieDTO);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }
}