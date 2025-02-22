using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
{
    private readonly AppDbContext _dbcontext;
    private readonly IMapper _mapper;
    private readonly IImageManagmentService _imageManagmentService;
    public ProductRepositry(AppDbContext dbcontext, IMapper mapper, IImageManagmentService imageManagmentService) : base(dbcontext)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
        _imageManagmentService = imageManagmentService;
    }

    public async Task<bool> AddAsync(AddProductDTO addProductDTO)
    {
        if (addProductDTO is null) return false;
        Product product=_mapper.Map<Product>(addProductDTO);
        await _dbcontext.Products.AddAsync(product);
        await _dbcontext.SaveChangesAsync();
        var ImagePath=await _imageManagmentService.AddImageAsync(addProductDTO.Photo, addProductDTO.Name);
        var Photo = ImagePath.Select(path => new Photo
        {
            ImageName=path,
            ProductId=product.Id
        }).ToList();
        await _dbcontext.Photos.AddRangeAsync(Photo);
        await _dbcontext.SaveChangesAsync();
        return true;
    }
}
