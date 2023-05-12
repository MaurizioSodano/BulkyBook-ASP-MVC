using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _UnitOfWork;


        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            this._UnitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {


            if (id == null || id == 0)
            {
                //create Company

                return View(new Company());
            }
            else
            {
                //update Company
                Company Company = _UnitOfWork.Company.Get(u => u.Id == id);
                if (Company == null)
                {
                    return NotFound();
                }
                return View(Company);
            }

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {

            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    _UnitOfWork.Company.Add(obj);
                }
                else
                {
                    _UnitOfWork.Company.Update(obj);
                }

                _UnitOfWork.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _UnitOfWork.Company.GetAll();//.Include(u => u.Category).Include(u => u.CoverType);
            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.Company.Remove(obj);
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }


}
