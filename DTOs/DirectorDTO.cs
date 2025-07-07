using System;
using System.ComponentModel.DataAnnotations;

public class DirectorDTO
{
    public int DirectorId { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Surname { get; set; } = string.Empty;
    
    [Required]
    public string Country { get; set; } = string.Empty;
    
    public DateTime? BirthDate { get; set; }

    public DirectorDTO() { }

    public DirectorDTO(Director director)
    {
        DirectorId = director.DirectorId;
        Name = director.Name;
        Surname = director.Surname;
        Country = director.Country;
        BirthDate = director.BirthDate;
    }
}