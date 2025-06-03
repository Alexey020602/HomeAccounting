using Checks.DataBase;
using FnsChecksApi;
using FnsChecksApi.Requests;
using Microsoft.EntityFrameworkCore;
using Refit;
using SkiaSharp;
using ZXing.SkiaSharp;

var path = "/Users/Alexey/Downloads/2025-01-24 17.40.56.jpg";
var file = new FileInfo(path);

if (!file.Exists) throw new Exception();
// SKBitmap bitmap = SKBitmap.Decode(path);
using var stream = file.Open(FileMode.Open);
var memory = new byte[stream.Length];
await stream.ReadExactlyAsync(memory, 0, memory.Length);
var image = SKImage.FromEncodedData(memory);
var bitmap = SKBitmap.FromImage(image);
var reader = new BarcodeReader();
var result = reader.Decode(bitmap);
Console.WriteLine(result.Text);
// var bitmap = 
var options = new DbContextOptionsBuilder<ApplicationContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .Options;

await using var context = new ApplicationContext(options);

// using var context = ApplicationContext(
//         new DbContextOptionsBuilder<ApplicationContext>()
//             .UseInMemoryDatabase(Guid.NewGuid().ToString())
//             .Options);

var checkService = RestService.For<ICheckService>("https://proverkacheka.com");
var receiptService = RestService.For<IReceiptService>("https://cheicheck.ru");

// var checkUseCase = new CheckUseCase(checkService, receiptService, context);

var requests = new[]
{
    // "t=20250107T104157&s=1691.16&fn=7380440801290534&i=9446&fp=1880975916&n=1",
    // "t=20250112T1715&s=93.00&fn=7381440800435707&i=21728&fp=3846028993&n=1",
    // "t=20250113T2321&s=1015.73&fn=7284440500173838&i=62887&fp=182171056&n=1",
    // "t=20250111T105907&s=1236.35&fn=7380440801290534&i=10277&fp=840967215&n=1",
    "t=20250104T1233&s=1289.00&fn=7380440700673345&i=26636&fp=1241320200&n=1"
}.Select(raw => new CheckRawRequest(raw)).ToList();

// var checkRequest = new CheckRequest
// {
//     Fd = "9152",
//     Fn = "7380440801290534",
//     Fp = "3909637264",
//     T = "20250105T1520",
//     S = "860.88"
// };
// requests.Add(checkRequest.RawRequest());
//
// await checkUseCase.SaveCheck(checkRequest.RawRequest());

// foreach (var request in requests)
// {
//     Console.WriteLine(request);
//     await checkUseCase.SaveCheck(request);
// }

var checks = await context.Checks
        .Include(c => c.Products)
        .ThenInclude(p => p.Subcategory)
        .ThenInclude(s => s.Products)
        .ToListAsync()
    ;

foreach (var check in checks) Console.WriteLine(check);