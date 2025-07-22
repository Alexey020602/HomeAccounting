namespace Authorization.Contracts;


public sealed class UserException : Exception
{
    public UserException(string message) : base(message) { }
    public UserException(string message, Exception innerException) : base(message, innerException) { }

    public static UserException NoFullName => new ("Отсутствует имя пользователя");
    public static UserException NoUserName => new ("Имя пользователя отсутствует");

    public static UserException NoUserid => new("Отсутствует идентификатор пользователя");
}