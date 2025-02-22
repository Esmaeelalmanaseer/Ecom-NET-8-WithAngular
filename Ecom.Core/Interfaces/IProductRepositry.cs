using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepositry:IGenericRepositry<Product>
    {
        Task<bool> AddAsync(AddProductDTO addProductDTO);
    }
}
