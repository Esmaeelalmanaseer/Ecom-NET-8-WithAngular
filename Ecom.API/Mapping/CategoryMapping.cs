using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping;

public class CategoryMapping:Profile
{
    public CategoryMapping()
    {
        CreateMap<Category,CategoryDTO>().ReverseMap();
        CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
    }
}