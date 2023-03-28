using BulkyBook.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _db.CoverTypes;
            return View(objCoverTypeList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _db.CoverTypes.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "CoverType created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _db.CoverTypes.Find(id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _db.CoverTypes.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "CoverType updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _db.CoverTypes.Find(id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.CoverTypes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.CoverTypes.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
