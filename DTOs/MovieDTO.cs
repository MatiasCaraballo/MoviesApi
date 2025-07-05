public class MovieDTO
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Classification { get; set; }
    public decimal? ImdbRating { get; set; }

    public MovieDTO() { }

    public MovieDTO(Movie movie)
    {
        Id = movie.MovieId;
        Name = movie.Name;
        ReleaseDate = movie.ReleaseDate;
        Classification = movie.Classification;
        ImdbRating = movie.ImdbRating;
    }

    public object?[] ToArray()
    {
        return new object?[]
        {
            Id,
            Name,
            ReleaseDate,
            Classification,
            ImdbRating
        };
    }
}