using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Service;

public class ImageManagmentService : IImageManagmentService
{
    private readonly IFileProvider _fileProvider;


    public ImageManagmentService(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<List<string>> AddImageAsync(IFormFileCollection file, string src)
    {
        var SaveImageSrc = new List<string>();
        var ImageDirctory = Path.Combine("wwwroot", "Images", src);
        if (!Directory.Exists(ImageDirctory))
        {
            Directory.CreateDirectory(ImageDirctory);
        }

        foreach (var item in file)
        {
            if (item.Length > 0)
            {
                var ImageName = item.Name;
                var ImageSrc = $"/Image/{src}/{ImageName}";
                var root = Path.Combine(ImageDirctory, ImageName);
                using (FileStream strem = new FileStream(root, FileMode.Create))
                {
                    await strem.CopyToAsync(strem);
                }
                SaveImageSrc.Add(ImageSrc);
            }
        }
        return SaveImageSrc;
    }

    public async Task DeleteImageAsync(string src)
    {
        var info= _fileProvider.GetFileInfo(src);
        if (info != null)
        {
            var root=info.PhysicalPath;
            File.Delete(root!);
        }
       await Task.CompletedTask;
    }
}
