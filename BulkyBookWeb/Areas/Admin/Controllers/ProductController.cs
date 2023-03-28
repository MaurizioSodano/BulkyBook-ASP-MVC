using BulkyBook.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Products;
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _db.Categories.Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _db.CoverTypes.Select( //Projection
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };


            if (id == null || id == 0)
            {
                //create product

                return View(productVM);
            }
            else
            {
                //update product
                //var productFromDb = _db.Products.Find(id);
                //if (productFromDb == null)
                //{
                //    return NotFound();
                //}
            }
            return View(productVM);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product obj)
        {

            if (ModelState.IsValid)
            {
                var productFromDb = _db.Products.Find(obj.Id);
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

                }
                //_db.Products.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Product updated successfully";
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
            var productFromDb = _db.Products.Find(id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Products.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
