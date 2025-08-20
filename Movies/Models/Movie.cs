
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public int MovieId { get; set; }

    [Required]
    public string Name { get; set; }

    public int ReleaseYear { get; set; }

    [Required]
    public string Classification { get; set; }

    [Required]
    public string Synopsis { get; set; }

    public decimal? ImdbRating { get; set; }

    public ICollection<Director> Directors { get; set; } = new List<Director>();

    public Movie() { }
    public Movie(string name, int releaseYear, string classification,string synopsis, decimal? imdbRating)
    {
        this.Name = name;
        this.ReleaseYear = releaseYear;
        this.Classification = classification;
        this.Synopsis = synopsis;
        this.ImdbRating = imdbRating;
    }
}
