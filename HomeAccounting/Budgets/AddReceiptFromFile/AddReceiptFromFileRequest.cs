using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Budgets.AddReceiptFromFile;

internal sealed record AddReceiptFromFileRequest([FromForm] IFormFile File);