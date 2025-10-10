using System.ComponentModel.DataAnnotations;

namespace ClientServerContracts.User.Login;

public record LoginRequest(string Login, string Password);