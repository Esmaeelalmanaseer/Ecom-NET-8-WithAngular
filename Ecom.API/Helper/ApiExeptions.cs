namespace Ecom.API.Helper;

public class ApiExeptions : ResponseAPI
{
    public string Details { get; set; }
    public ApiExeptions(int statusCode, string message = null, string details = null) : base(statusCode, message)
    {
        Details = details;
    }
}