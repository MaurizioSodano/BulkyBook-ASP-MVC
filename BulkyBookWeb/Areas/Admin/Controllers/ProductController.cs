using BulkyBook.Data;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ApplicationDbContext applicationDbContext, IWebHostEnvironment hostEnvironment)
        {
            _db = applicationDbContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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
                productVM.Product = _db.Products.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
            }
            return View(productVM);
        }

    //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString(); //creates unique fileName
                    var uploads = Path.Combine(wwwRootPath, @"images\products");  //concatenates the folder path
                    var extension = Path.GetExtension(file.FileName); // get the original file Extension

                    //check if there is an existing image
                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams); //copy the file
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
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
                        productFromDb.ImageUrl = obj.Product.ImageUrl;
                    }

                }
                else
                {
                    _db.Products.Add(obj.Product);
                }
                //_db.Products.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _db.Products.Include(u => u.Category).Include(u => u.CoverType);
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _db.Products.Remove(obj);
            _db.SaveChanges();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }


}
