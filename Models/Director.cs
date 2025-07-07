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

    public DateTime? BirthDate { get; set; }


    //Secret Data
    public DateTime? CreatedAt { get; set; }


    //Many relationship
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public Director() { }
    public Director(string name, string surname, string country, DateTime birthDate)
    {
        this.Name = name;
        this.Surname = surname;
        this.Country = country;
        this.BirthDate = birthDate;
    }

}