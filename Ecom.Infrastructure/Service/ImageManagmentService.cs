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

        // إنشاء المجلد إذا لم يكن موجودًا
        if (!Directory.Exists(ImageDirctory))
        {
            Directory.CreateDirectory(ImageDirctory);
        }

        foreach (var item in file)
        {
            if (item.Length > 0)
            {
                var ImageName = item.FileName;
                var ImageSrc = $"/Images/{src}/{ImageName}";
                var root = Path.Combine(ImageDirctory, ImageName);

                // استخدام using لفتح FileStream للكتابة
                using (var stream = new FileStream(root, FileMode.Create))
                {
                    await item.CopyToAsync(stream); // نسخ الملف من IFormFile إلى FileStream
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
            try
            {
                if (File.Exists(root))
                {
                    File.Delete(root);
                }
            }
            catch (IOException ex)
            {
                // تعامل مع الاستثناء (مثل تسجيله أو إعادة المحاولة)
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
       await Task.CompletedTask;
    }
}
