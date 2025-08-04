using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/directors")]
[ApiController]

public class DirectorController : ControllerBase
{
    private readonly IDirectorService _directorService;

    public DirectorController(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet("Directors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Directors")]

    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllDirectors()
    {
        try
        {
            var movies = await _directorService.GetAllDirectors();
            return Ok(movies);

        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");

        }
    }

    [HttpGet("Directors/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Directors")]
    [Authorize(Roles = "Admin")]


    public async Task<ActionResult<DirectorDTO>> GetDirector(int id)
    {
        try
        {
            var director = await _directorService.GetDirector(id);

            if (director == null)
                return NotFound();

            return Ok(director);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpPost("Directors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Directors")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DirectorDTO>> PostDirector(DirectorDTO directorDTO)
    {
        try
        {
            var director = await _directorService.PostDirector(directorDTO);

            if (director == null)
                return NotFound();

            return Ok(director);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }

}