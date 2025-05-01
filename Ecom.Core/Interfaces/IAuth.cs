using Ecom.Core.DTO;

namespace Ecom.Core.Interfaces;

public interface IAuth
{
    Task<string?> RegisterAsync(RegisterDTO registerDTO);
    Task<string?> LoginAsync(LoginDTO LoginDTO);
    Task<bool> SendEmailForForgetPassword(string email);
    Task<string?> ResetPassword(ResetPasswordDTO resetPasswordDTO);
    Task<bool> ActiveAccount(ActiveAccountDTO ActiveAccountdto);
    Task SendEmail(string email, string code, string component, string subject, string message);
}
