using System.Text.Json;
using FnsChecksApi;
using FnsChecksApi.Requests;
using Refit;

var options = new JsonSerializerOptions();
var serializer = new SystemTextJsonContentSerializer();

var settings = new RefitSettings()
{
    ContentSerializer = serializer,
};

var service = RestService.For<ICheckService>("https://proverkacheka.com");
const string token = "15239.20dUQQYmlHxbOPLzb";
var request = new CheckRequest
{
    Token = token,
    Fd = "8622",
    Fn = "7380440801290534",
    Fp = "1229409814",
};

var file = new FileInfo(@"C:\Users\Fedor\Downloads\photo_2025-01-03_17-10-45.jpg");
var json = await service.GetAsyncByFile(file).ConfigureAwait(false);
// var json = await service.GetAsyncByRaw(request);
foreach (var item in json.Data.Json.Items)
{
    Console.WriteLine(item);
}

// foreach (var product in json.Goods)
// {
//     Console.WriteLine(product);
// }
// int type;
int.TryParse(json.Request.Manual.Type, out var type);

Console.WriteLine(type);

Console.WriteLine(json);