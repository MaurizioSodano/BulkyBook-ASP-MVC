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
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var productFromDb = _db.Products.Find(obj.Product.Id);
                if (productFromDb != null)
                {
                    productFromDb.Title = obj.Product.Title;
                    productFromDb.ISBN = obj.Product.ISBN;
                    productFromDb.ListPrice = obj.Product.ListPrice;
                    productFromDb.Price = obj.Product.Price;
                    productFromDb.Price50 = obj.Product.Price50;
                    productFromDb.Price100 = obj.Product.Price100;
                    productFromDb.Description = obj.Product.Description;
                    productFromDb.CategoryId = obj.Product.CategoryId;
                    productFromDb.Author = obj.Product.Author;
                    productFromDb.CoverTypeId = obj.Product.CoverTypeId;
                    if (file != null)
                    {
                        productFromDb.ImageUrl = file.FileName;
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
