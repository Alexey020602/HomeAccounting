namespace Core.Model;

public class User(string login, string name)
{
    public static User Default => new User(Guid.Empty.ToString(), "Пользователь не выбран");
    public string Login { get; set; } = login;
    public string Name { get; set; } = name;
}