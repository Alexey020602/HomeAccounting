namespace Authorization.Core;

public class UnregisteredUser
{
    public required string Login { get; init; }
    public required string UserName { get; init; }
}