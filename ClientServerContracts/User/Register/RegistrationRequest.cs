namespace ClientServerContracts.User.Register;

public sealed record RegistrationRequest(string Login, string Password, string UserName);