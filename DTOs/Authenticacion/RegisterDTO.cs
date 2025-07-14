using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } 

    [MaxLength(100)]
    public string UserName { get; set; }
    
    [MinLength(6)]
    public string Password { get; set; }

    
}