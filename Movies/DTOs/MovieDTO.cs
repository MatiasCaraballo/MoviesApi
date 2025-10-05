public class MovieDTO
{
    public int? MovieId { get; set; }
    public string? Name { get; set; }
    public int ReleaseYear { get; set; }
    public string? Classification { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    public decimal? ImdbRating { get; set; }

    public List<string> Directors { get; set; } = new List<string>();

    public MovieDTO() { }

    public MovieDTO(Movie movie)
    {
        MovieId = movie.MovieId;
        Name = movie.Name;
        ReleaseYear = movie.ReleaseYear;
        Classification = movie.Classification;
        Synopsis = movie.Synopsis;
        ImdbRating = movie.ImdbRating;
        Directors = movie.Directors
                         .Select(d => d.Name + " " + d.Surname)
                         .ToList();
    }
}

public class PostMovieDTO
{
    public string? Name { get; set; }
    public int ReleaseYear { get; set; }
    public string? Classification { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    public decimal? ImdbRating { get; set; }
    public PostMovieDTO() { }

    public PostMovieDTO(Movie movie)
    {
        Name = movie.Name;
        ReleaseYear = movie.ReleaseYear;
        Classification = movie.Classification;
        Synopsis = movie.Synopsis;
        ImdbRating = movie.ImdbRating;
    }
}