using System.ComponentModel.DataAnnotations;

namespace Core.Authorization;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public required string Login { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}