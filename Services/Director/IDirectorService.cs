public interface IDirectorService
{
    Task<IEnumerable<DirectorDTO>> GetAllDirectors();
    Task<DirectorDTO> GetDirector(int id);
    Task<IResult> PostDirector(DirectorDTO directorDTO);



}