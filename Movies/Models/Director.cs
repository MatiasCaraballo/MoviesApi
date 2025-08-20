using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

public class Director
{

    public int DirectorId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    public int? BirthYear { get; set; }

    //Many relationship
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public Director() { }
    public Director(string name, string surname, string country, int? birthYear)
    {
        this.Name = name;
        this.Surname = surname;
        this.Country = country;
        this.BirthYear = birthYear;
    }

}