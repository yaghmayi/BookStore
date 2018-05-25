using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LightStore.DataAccess;
using LightStore.Models;
//using LightStore.Web.Controllers;
using LightStore.UnitTest.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Web.Controllers;

namespace LightStore.UnitTest.Web
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            TestUtil.ClearTable("Products");
            TestUtil.ClearTable("Categories");

            CategoryDAO.Save(new Category() { Code = 1, Name = "Electronical" });
            Category labTopCategory = new Category() { Code = 2, Name = "LabTop", ParentCode = 1 };
            CategoryDAO.Save(labTopCategory);

            CategoryDAO.Save(new Category() { Code = 3, Name = "Furniture" });

            ProductDAO.Save(new Product() { Code = 100, Title = "Lenovo 1020", Category = labTopCategory });
        }


        [TestMethod]
        public void ListCategories()
        {
            ProductController controller = new ProductController();
            ViewResult view = controller.ListCategories() as ViewResult;
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.Model);
            Assert.IsTrue(view.Model is List<CategoryViewModel>);

            List<CategoryViewModel> models = (List<CategoryViewModel>) view.Model;
            Assert.AreEqual(2, models.Count);
            Assert.AreEqual("Electronical", models.First().Name);
            Assert.AreEqual(1, models.First().SubCategories.Count);
            Assert.AreEqual("LabTop", models.First().SubCategories.First().Name);
            Assert.AreEqual(1, models.First().SubCategories.First().ProductsNumber);
        }

        [TestMethod]
        public void ShowCategory()
        {
            ProductController controller = new ProductController();
            ViewResult view = controller.ShowCategory(1) as ViewResult;
            Assert.IsNotNull(view.Model);
            Assert.IsNotNull(view.Model is Category);
            Category category = (Category) view.Model;
            Assert.AreEqual(1, category.Code);
            Assert.AreEqual("Electronical", category.Name);
        }

        public void CreateCategory()
        {
//            ProductController controller = new ProductController();
//            ViewResult view = (ViewResult) controller.CreateCategory((int?) null);

        }

        public void ListProducts()
        {
//            ProductController controller = new ProductController();
//            controller.Url  = new UrlHelper();
//            controller.Request = new HttpRequestMessage()
//            {
//                RequestUri = new Uri("http://localhost:13985/Product/ListProducts?categoryCode=4")
//            };
            
//            ActionResult listProducts = controller.ListProducts(4);
//            ControllerContext controllerContext = new ControllerContext();
//            listProducts.ExecuteResult(controllerContext);
        }
    }
}
