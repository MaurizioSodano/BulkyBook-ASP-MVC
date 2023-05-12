using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
