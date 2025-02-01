namespace Core.Model.Requests;

public class FileCheckRequest
{
    public Stream FileStream { get; init; } = null!;
    public DateTimeOffset AddedTime { get; set; }
}