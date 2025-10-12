namespace ClientServerContracts.User.Register;

public sealed record RegistrationRequest(string UserName, string Password, string FullName);