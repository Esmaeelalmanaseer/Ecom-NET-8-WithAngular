using Ecom.Core.DTO;

namespace Ecom.Core.Services;

public interface IEmailService
{
    Task SendEmail(EmailDTO email); 
}
