namespace ClientServerShared;

public static class UriExtensions
{
    public static Uri AppendingPath(this Uri uri, string? path)
    {
        if (path is null) return uri;

        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Path += path;
        return uriBuilder.Uri;
    }
}