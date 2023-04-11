using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext _db) : base(_db)
        {
        }


        public void Update(Product obj)
        {
            var productFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (productFromDb != null)
            {
                productFromDb.Title = obj.Title;
                productFromDb.ISBN = obj.ISBN;
                productFromDb.ListPrice = obj.ListPrice;
                productFromDb.Price = obj.Price;
                productFromDb.Price50 = obj.Price50;
                productFromDb.Price100 = obj.Price100;
                productFromDb.Description = obj.Description;
                productFromDb.CategoryId = obj.CategoryId;
                productFromDb.Author = obj.Author;
                productFromDb.CoverTypeId = obj.CoverTypeId;
                if (obj.ImageUrl != null)
                {
                    productFromDb.ImageUrl = obj.ImageUrl;
                }

                _db.Products.Update(productFromDb);
            }
        }

    }
}
