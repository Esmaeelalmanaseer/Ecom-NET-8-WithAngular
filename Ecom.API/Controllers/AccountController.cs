using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;

public class AccountController : BaseController
{
    public AccountController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    [HttpPost("Register")]
    public async Task<IActionResult> register(RegisterDTO registerDTO)
    {
        try
        {
            var result = await _unitOfWork.Auth.RegisterAsync(registerDTO);
            if (result is not "done") return BadRequest(new ResponseAPI(400, result!));
            return Ok(new ResponseAPI(200, result));
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw;
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> login(LoginDTO loginDTO)
    {
        try
        {
            var result = await _unitOfWork.Auth.LoginAsync(loginDTO);
            if (result is null || result.StartsWith("Please")) return BadRequest(new ResponseAPI(400, result.Any() ? result : "User Not Found"));
            Response.Cookies.Append("token", result, new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });
            return Ok(new ResponseAPI(200));
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw;
        }
    }

    [HttpPost("active-account")]
    public async Task<IActionResult> active(ActiveAccountDTO activeAccountDTO)
    {
        try
        {
            var result = await _unitOfWork.Auth.ActiveAccount(activeAccountDTO);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw;
        }
    }

    [HttpPost("send-email-forget-password")]
    public async Task<IActionResult> forget(string email)
    {
        try
        {
            var result = await _unitOfWork.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw;
        }
    }
}
