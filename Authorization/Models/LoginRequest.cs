using System.ComponentModel.DataAnnotations;

namespace Authorization.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public required string Login { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}