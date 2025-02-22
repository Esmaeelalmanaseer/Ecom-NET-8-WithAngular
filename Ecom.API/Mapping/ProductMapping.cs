using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
            .ReverseMap();
        CreateMap<Photo, PhotoDTO>().ReverseMap();
        CreateMap<AddProductDTO, Product>()
            .ForMember(dest=>dest.Photos,opt=>opt.Ignore())
            .ReverseMap();
    }
}
