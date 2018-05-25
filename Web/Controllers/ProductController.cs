using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using LightStore.DataAccess;
using LightStore.Models;
using LightStore.Web.Controllers.Base;

namespace LightStore.Web.Controllers
{
    public partial class ProductController : Controller
    {
        [HttpGet]
        public void AddShopItem(int id)
        {
            if (Session[DataKeys.ShopItems] == null)
                Session[DataKeys.ShopItems] = new List<int>();
            if (Session[DataKeys.ShopItemsCount] == null)
                Session[DataKeys.ShopItemsCount] = 0;

            List<int> shopItemCodes = (List<int>)Session[DataKeys.ShopItems];
            shopItemCodes.Add(id);

            int count = (int)Session[DataKeys.ShopItemsCount];
            Session[DataKeys.ShopItemsCount] = count + 1;
        }

        [HttpGet]
        public ActionResult ListProducts()
        {
            BookSearch productSearch = new BookSearch() { };
            return ListProducts(productSearch);
        }


        [HttpPost]
        public ActionResult ListProducts(BookSearch productSearch)
        {
            List<Book> products = BookDAO.Search(productSearch);
            ViewData[DataKeys.Products] = products;

            return View(productSearch);
        }

        [HttpGet]
        public ActionResult Foo()
        {
            return View();
//            return ListProducts(new ProductSearch());
//            return View(new ProductSearch());
        }

        [HttpPost]
        public ActionResult Foo(BookSearch productSearch)
        {
//            productSearch.Title = "Foo";
//            SetSession("MaxPrice", productSearch.MaxPrice.ToString());
//            return View(productSearch);
            return View(productSearch);
        }


        public ActionResult ShowProduct(int id)
        {
            Book product = BookDAO.Get(id);
            return View(product);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhoto(int id)
        {
            Book product = BookDAO.Get(id);
            return new ImageResult(product.Pic);
        }

        public string GetSession(string id)
        {
            if (Session[id] != null)
                return Session[id].ToString();
            else
                return null;
        }

        public void SetSession(string id, string value)
        {
            Session[id] = value;
        }

        public ActionResult Test()
        {
//            return Redirect("Product/CreateProduct?categoryCode=105");
//              return Redirect("Product/ListProducts?categoryCode=105");
//            return Redirect("Product/ShowProduct/32");
//            return Redirect("Product/Foo");
//            return Redirect("Product/ListCategories");
            return Redirect("Product/ListProducts?categoryCode=2");
//            return Redirect("Product/Foo");
//            return Redirect("Customer/FinishShop");
//            return View();
        }
    }
}
