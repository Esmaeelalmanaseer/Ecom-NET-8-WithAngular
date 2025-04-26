using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService _imageManagmentService;
        private readonly IConnectionMultiplexer _redis;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManger;
        public IProductRepositry ProductRepositry { get; }

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public ICustomerBasketRepositry CustomerBasket { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext appDbContext, IMapper mapper, IImageManagmentService imageManagmentService, IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _imageManagmentService = imageManagmentService;
            _redis = redis;
            _userManager = userManager;
            _emailService = emailService;
            _signInManger = signInManger;
            ProductRepositry = new ProductRepositry(_appDbContext, _mapper, _imageManagmentService);
            CategoryRepositry = new CategoryRepositry(_appDbContext);
            PhotoRepositry = new PhotoRepositry(_appDbContext);
            CustomerBasket = new CustomerBasketRepositry(_redis!);
            Auth = new AuthRepositry(_userManager, _emailService, _signInManger);
        }
    }
}