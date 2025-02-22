using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService _imageManagmentService;
        public IProductRepositry ProductRepositry { get; }

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public UnitOfWork(AppDbContext appDbContext, IMapper mapper, IImageManagmentService imageManagmentService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _imageManagmentService = imageManagmentService;

            ProductRepositry = new ProductRepositry(_appDbContext,_mapper,_imageManagmentService);
            CategoryRepositry = new CategoryRepositry(_appDbContext);
            PhotoRepositry = new PhotoRepositry(_appDbContext);
        }
    }
}
