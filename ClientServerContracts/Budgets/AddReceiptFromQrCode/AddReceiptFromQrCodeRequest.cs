namespace ClientServerContracts.Budgets.AddReceiptFromQrCode;

/// <summary>
/// Request to add a receipt from raw QR code string (FNS fiscal format).
/// Format: t=20240101T1200&amp;s=12345&amp;fn=1234567890123456&amp;i=123&amp;fp=12345678&amp;n=1
/// </summary>
public sealed record AddReceiptFromQrCodeRequest(string Raw);
