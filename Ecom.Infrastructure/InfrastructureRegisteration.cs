using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Security.AccessControl;

namespace Ecom.Infrastructure;

public static class InfrastructureRegisteration
{
    public static IServiceCollection InfrastructureConfigration(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IImageManagmentService, ImageManagmentService>();
        services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot")));
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Ecom"));
        });
        return services;
    }
}