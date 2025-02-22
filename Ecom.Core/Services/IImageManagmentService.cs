using Microsoft.AspNetCore.Http;

namespace Ecom.Core.Services;

public interface IImageManagmentService
{
    Task<List<string>> AddImageAsync(IFormFileCollection file,string src);
    Task DeleteImageAsync(string src);
}
