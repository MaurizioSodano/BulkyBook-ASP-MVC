﻿using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        public CoverTypeRepository(ApplicationDbContext _db) : base(_db)
        {
        }



        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
