using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Shared.Web;

public static class CustomTypedResults
{
    public static InternalServerError UnknownResult() => TypedResults.InternalServerError();
}