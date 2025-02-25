namespace Core.Model;

public class User(string name)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
}