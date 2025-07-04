
public class MovieDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Genre { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
    public string? Classification { get; set; }

    public MovieDTO() { }

    public MovieDTO(Movie movie)
    {
        Id = movie.MovieId;
        Name = movie.Name;
        Genre = movie.Genre;
        ReleaseDate = movie.ReleaseDate;
        Classification = movie.Classification;
    }
}