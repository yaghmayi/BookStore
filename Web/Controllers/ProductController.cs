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
        public ActionResult ListProducts(int? categoryCode)
        {
            ProductSearch productSearch = new ProductSearch() { CategoryCode = categoryCode };
            productSearch.ParentCategoryCode = CategoryDAO.Get(productSearch.CategoryCode.Value).ParentCode;
            return ListProducts(productSearch);
        }


        [HttpPost]
        public ActionResult ListProducts(ProductSearch productSearch)
        {
            SetSession(DataKeys.MaxPrice, productSearch.MaxPrice.ToString());
            List<Product> products = ProductDAO.Search(productSearch);
//            List<Product> products = new List<Product>();
            if (productSearch.CategoryCode != null)
            {
                Category category = CategoryDAO.Get(productSearch.CategoryCode.Value);
                ViewData[DataKeys.Category] = category;
            }
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
        public ActionResult Foo(ProductSearch productSearch)
        {
//            productSearch.Title = "Foo";
//            SetSession("MaxPrice", productSearch.MaxPrice.ToString());
//            return View(productSearch);
            return View(productSearch);
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult CreateProduct(int categoryCode)
        {
            EditProductViewModel editProductViewModel = new EditProductViewModel();
            Category category = CategoryDAO.Get(categoryCode);
            editProductViewModel.CategoryCode = categoryCode;
            editProductViewModel.CategoryName = category.Name;
            editProductViewModel.Code = ProductDAO.GetNextCode();
            editProductViewModel.Title = "New Product";

            return View(editProductViewModel);
        }

        [HttpPost]
        public ActionResult CreateProduct(EditProductViewModel editProductViewModel)
        {
            Product product = MakeProduct(editProductViewModel);
            product.Pic = MakePicFromRequestFile("Pic");

            ProductDAO.Save(product);

            return RedirectToAction("ListProducts", "Product", new { categoryCode = product.Category.Code });
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult EditProduct(int id)
        {
            Product product = ProductDAO.Get(id);
            EditProductViewModel editProductViewModel = MakeEditProductViewModel(product);

            return View(editProductViewModel);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel editProductViewModel)
        {
            Product product = MakeProduct(editProductViewModel);
            byte[] pic = MakePicFromRequestFile("Pic");
            if (pic != null)
                product.Pic = pic;
            else
                product.Pic = ProductDAO.GetPic(product.Code);
            ProductDAO.Save(product);

            return RedirectToAction("ListProducts", "Product", new { categoryCode = product.Category.Code });
        }

        public ActionResult ShowProduct(int id)
        {
            Product product = ProductDAO.Get(id);
            return View(product);
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult DeleteProduct(int id)
        {
            Product product = ProductDAO.Get(id);
            EditProductViewModel editProductViewModel = MakeEditProductViewModel(product);
            return View(editProductViewModel);
        }

        [HttpPost]
        public ActionResult DeleteProduct(EditProductViewModel editProductViewModel)
        {
            ProductDAO.Delete(editProductViewModel.Code);
            return RedirectToAction("ListProducts", "Product", new { categoryCode = editProductViewModel.CategoryCode });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhoto(int id)
        {
            Product product = ProductDAO.Get(id);
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
