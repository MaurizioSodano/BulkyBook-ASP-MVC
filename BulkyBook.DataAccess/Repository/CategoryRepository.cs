using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
