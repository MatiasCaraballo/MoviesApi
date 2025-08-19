using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]

public class DirectorController : ControllerBase
{
    private readonly IDirectorService _directorService;

    public DirectorController(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetAllDirectors()
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

    [HttpGet("Director/{DirectorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    ///  <summary>
    /// Search Directors by Name,Movie or Country
    /// </summary>
    /// <param name="search"> Movie,Director name or Country.</param>
    [HttpGet("/Directors/{search}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    
    /// <returns> DirectorDTO </returns>
    public async Task<ActionResult<IEnumerable<DirectorDTO>>> SearchDirectors(string? search)
    {
        try
        {
            var directors = await _directorService.SearchDirectors(search);
            return Ok(directors);

        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");

        }
    }

}