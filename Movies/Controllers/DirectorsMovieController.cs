using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/directors-movies")]
[ApiController]

public class DirectorsMovieController : ControllerBase
{

    private readonly IDirectorsMovieService _directorsMovieService;

    public DirectorsMovieController(IDirectorsMovieService directorsMovieService)
    {
        _directorsMovieService = directorsMovieService;
    }

    [HttpGet("Directors-Movie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Directors-Movie")]

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
