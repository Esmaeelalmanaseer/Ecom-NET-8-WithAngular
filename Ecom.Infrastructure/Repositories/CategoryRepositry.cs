using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class CategoryRepositry : GenericRepositry<Category>,ICategoryRepositry
{
    public CategoryRepositry(AppDbContext dbcontext) : base(dbcontext)
    {
    }

}
