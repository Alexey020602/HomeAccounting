using System.ComponentModel.DataAnnotations;

namespace Authorization;

public record LoginRequest(
    [property: Required(ErrorMessage = "Username is required")]
    string Login,
    [property: Required(ErrorMessage = "Password is required")]
    string Password
    );