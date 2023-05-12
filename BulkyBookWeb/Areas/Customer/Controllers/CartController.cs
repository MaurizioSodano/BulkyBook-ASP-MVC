﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),

            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb != null)
            {
                cartFromDb.Count++;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb != null)
            {
                if (cartFromDb.Count <= 1)
                {
                    _unitOfWork.ShoppingCart.Remove(cartFromDb);
                }
                else
                {
                    cartFromDb.Count--;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }

                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb != null)
            {

                _unitOfWork.ShoppingCart.Remove(cartFromDb);



                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)

        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }

        }
    }
}
