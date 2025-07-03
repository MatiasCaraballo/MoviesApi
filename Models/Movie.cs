using System;
using System.Collections.Generic;

public class Movie
{
    public int MovieId { get; set; }

    public string Name { get; set; }

    public string Genre { get; set; } = string.Empty;

    public DateTime? ReleaseDate { get; set; }

    public string Classification { get; set; }

    //Secret Data
    public DateTime? CreatedAt { get; set; }
    
    public ICollection<Director> Directors { get; set; } = new List<Director>();
    public Movie(int movieId, string name,string genre, DateTime? releaseDate, string classification, DateTime? createdAt)
    {
        this.MovieId = movieId;
        this.Name = name;
        this.Genre = genre;
        this.ReleaseDate = releaseDate;
        this.Classification = classification;
        this.CreatedAt = createdAt;
    }
}
