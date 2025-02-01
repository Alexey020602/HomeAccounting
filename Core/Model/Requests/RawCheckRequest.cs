namespace Core.Model.Requests;

public class RawCheckRequest
{
    public required string QrRaw { get; init; }
    public required DateTimeOffset AddedTime { get; init; }
}