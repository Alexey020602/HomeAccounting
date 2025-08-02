namespace Authorization.Core;

internal sealed class TokenSettingsException : Exception
{
    public TokenSettingsException(string message) : base(message) { }
    public TokenSettingsException(string message, Exception innerException) : base(message, innerException) { }
}