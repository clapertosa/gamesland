using System.Net;

namespace GamesLand.Core.Errors;

public class RestException : Exception
{
    public RestException(HttpStatusCode statusCode, object errors)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public HttpStatusCode StatusCode { get; set; }
    public object Errors { get; set; }
}