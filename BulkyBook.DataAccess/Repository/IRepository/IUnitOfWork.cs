﻿namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICoverTypeRepository CoverType { get; }
        ICompanyRepository Company { get; }

        IShoppingCartRepository ShoppingCart { get; }
        void Save();

    }
}
