using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]

public class DirectorsMovieController : ControllerBase
{

    private readonly IDirectorsMovieService _directorsMovieService;

    public DirectorsMovieController(IDirectorsMovieService directorsMovieService)
    {
        _directorsMovieService = directorsMovieService;
    }

    [HttpGet("/Director-Movie/{movieId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirectorsByMovie(int movieId)
    {
        try
        {
            var directors = await _directorsMovieService.GetDirectorsByMovie(movieId);
            return Ok(directors);

        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");

        }
    }

}
