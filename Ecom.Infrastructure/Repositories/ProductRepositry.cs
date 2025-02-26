using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        Product product = _mapper.Map<Product>(addProductDTO);
        await _dbcontext.Products.AddAsync(product);
        await _dbcontext.SaveChangesAsync();
        var ImagePath = await _imageManagmentService.AddImageAsync(addProductDTO.Photo, addProductDTO.Name);
        var Photo = ImagePath.Select(path => new Photo
        {
            ImageName = path,
            ProductId = product.Id
        }).ToList();
        await _dbcontext.Photos.AddRangeAsync(Photo);
        await _dbcontext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Product productObj)
    {
        var photo = await _dbcontext.Photos.Where(p => p.ProductId == productObj.Id).ToListAsync();
        if (photo.Any())
        {
            foreach(var item in photo)
            {
                await _imageManagmentService.DeleteImageAsync(item.ImageName);
            }
        }
        _dbcontext.Products.Remove(productObj);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(UpdateProductDTO updateProduct)
    {
        if (updateProduct is null) return false;

        var FindProduct = await _dbcontext.Products.Include(c => c.Category).Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.Id == updateProduct.Id);
        if (FindProduct is null) return false;

        _mapper.Map(updateProduct, FindProduct);

        var photo = await _dbcontext.Photos.Where(m => m.ProductId == updateProduct.Id).ToListAsync();

        if (photo.Any())
        {
            foreach (var item in photo)
            {
                await _imageManagmentService.DeleteImageAsync(item.ImageName);
            }
            _dbcontext.Photos.RemoveRange(photo);

            var ImagePath = await _imageManagmentService.AddImageAsync(updateProduct.Photo, updateProduct.Name);
            var photosave = ImagePath.Select(x => new Photo
            {
                ImageName = x,
                ProductId = updateProduct.Id
            }).ToList();

            await _dbcontext.Photos.AddRangeAsync(photosave);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
        return true;
    }
}
