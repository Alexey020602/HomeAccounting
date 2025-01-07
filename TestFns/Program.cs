using System.Drawing;
using System.Text.Json;
using FnsChecksApi;
using FnsChecksApi.Dto;
using FnsChecksApi.Requests;
using Refit;
using ZXing;
using ZXing.CoreCompat.System.Drawing;

var options = new JsonSerializerOptions();
var serializer = new SystemTextJsonContentSerializer();

var settings = new RefitSettings()
{
    ContentSerializer = serializer,
};

var checkImagePath = @"C:\Users\Fedor\Downloads\photo_2025-01-07_11-55-29.jpg";

var service = RestService.For<ICheckService>("https://proverkacheka.com");
const string token = "15239.20dUQQYmlHxbOPLzb";
var request = new CheckRequest
{
    Token = token,
    Fd = "70399",
    Fn = "7380440800793300",
    Fp = "4259975605",
    T = "07.01.25 10:57",
    S = "444.95",
};

var checkRawData = "t=20250105T152050&s=860.88&fn=7380440801290534&i=9152&fp=3909637264&n=1";

var rawRequest = new CheckRawRequest(checkRawData);

IBarcodeReader<Bitmap> reader = new BarcodeReader();
var image = Image.FromFile(checkImagePath);
var barcodeBitmap = (Bitmap) image;

var result = reader.Decode(barcodeBitmap);

Console.WriteLine(result.Text);
// var dictionary = HttpUtility.ParseQueryString(checkRawData);
//
// string json = JsonSerializer.Serialize(dictionary.Cast<string>().ToDictionary(k => k, v => dictionary[v]));
//
// CheckRequest checkRequest = JsonSerializer.Deserialize<CheckRequest>(json, options);

// var checkRequest = new CheckRequest()
// {   
//     Fd = dictionary["i"] ?? throw new ArgumentNullException(),
//     Fn = dictionary["fn"] ?? throw new ArgumentNullException(),
//     Fp = dictionary["fp"] ?? throw new ArgumentNullException(),
//     T = dictionary["t"] ?? throw new ArgumentNullException(),
//     S = dictionary["s"] ?? throw new ArgumentNullException(),
// };

var file = new FileInfo(checkImagePath);
var receipt = await service.GetAsyncByRaw(rawRequest);

// var receipt = await service.GetAsyncByFile(file);
if (receipt is not Root root)
{
    return;
}
// var json = await service.GetAsyncByRaw(request);
foreach (var item in root.Data.Json.Items)
{
    Console.WriteLine(item);
}

// foreach (var product in json.Goods)
// {
//     Console.WriteLine(product);
// }
// int type;
int.TryParse(root.Request.Manual.Type, out var type);

Console.WriteLine(type);

Console.WriteLine(root);
