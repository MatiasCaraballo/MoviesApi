
public class Movie
{
    public int MovieId { get; set; }

    public string Name { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string Classification { get; set; }


    public decimal? ImdbRating { get; set; }

    //Secret Data
    public DateTime? CreatedAt { get; set; }

    public ICollection<Director> Directors { get; set; } = new List<Director>();

    public Movie() { }
    public Movie(string name, DateTime? releaseDate, string classification, decimal? imdbRating, DateTime? createdAt)
    {
        this.Name = name;
        this.ReleaseDate = releaseDate;
        this.Classification = classification;
        this.ImdbRating = imdbRating;
        this.CreatedAt = createdAt;
    }
}
