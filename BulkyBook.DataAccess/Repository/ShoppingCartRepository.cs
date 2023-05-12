using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext _db) : base(_db)
        {
        }


        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }

    }
}
