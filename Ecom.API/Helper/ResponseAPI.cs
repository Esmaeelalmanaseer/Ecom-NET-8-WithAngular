namespace Ecom.API.Helper;

public class ResponseAPI
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public ResponseAPI(int statusCode, string message = null!)
    {
        StatusCode = statusCode;
        Message = message ?? GetMassegeFromStatusCode(StatusCode)!;
    }
    private string? GetMassegeFromStatusCode(int statuscode)
    {
        return statuscode switch
        {
            200 => "Done",
            400 => "Bad Request",
            401 => "Un Authorized",
            404 =>"Not Found",
            500 => "Server Error",
            _ => null
        };
    }
}