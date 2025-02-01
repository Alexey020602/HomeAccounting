namespace Core.Model.Requests;

public class FileCheckRequest
{
    public Stream FileStream { get; init; } = null!;
    public DateTime AddedTime { get; set; }
}