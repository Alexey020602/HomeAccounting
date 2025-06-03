using System.ComponentModel.DataAnnotations;

namespace Authorization.Dto;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string Login { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public override string ToString() => $"Login: {Login}, Password: {Password}";
}