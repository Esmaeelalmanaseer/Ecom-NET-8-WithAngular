using Ecom.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("error/{statuscode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int statuscode)
        {
            return new ObjectResult(new ResponseAPI(statuscode));
        }
    }
}
