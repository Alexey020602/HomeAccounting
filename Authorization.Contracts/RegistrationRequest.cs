namespace Authorization.Contracts;

public class RegistrationRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
    public required string Username { get; init; }
}