﻿namespace Ecom.Core.Interfaces;

public interface IUnitOfWork
{
    public IProductRepositry ProductRepositry { get; }
    public ICategoryRepositry CategoryRepositry { get; }
    public IPhotoRepositry PhotoRepositry { get; }
}