using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        public IProductRepositry ProductRepositry { get; }

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            ProductRepositry = new ProductRepositry(_appDbContext);
            CategoryRepositry = new CategoryRepositry(_appDbContext);
            PhotoRepositry = new PhotoRepositry(_appDbContext);
        }
    }
}
