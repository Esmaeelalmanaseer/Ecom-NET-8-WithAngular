using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
{
    public ProductRepositry(AppDbContext dbcontext) : base(dbcontext)
    {
    }
}
