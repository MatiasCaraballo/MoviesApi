public class MovieDTO
{
    public int? MovieId { get; set; }
    public string? Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Classification { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    public decimal? ImdbRating { get; set; }

    public MovieDTO() { }

    public MovieDTO(Movie movie)
    {
        MovieId = movie.MovieId;
        Name = movie.Name;
        ReleaseDate = movie.ReleaseDate;
        Classification = movie.Classification;
        Synopsis = movie.Synopsis;
        ImdbRating = movie.ImdbRating;
    }

    public object?[] ToArray()
    {
        return new object?[]
        {
            MovieId,
            Name,
            ReleaseDate,
            Classification,
            Synopsis,
            ImdbRating
        };
    }
}