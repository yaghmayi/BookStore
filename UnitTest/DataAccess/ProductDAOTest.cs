using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls.Expressions;
using LightStore.DataAccess;
using LightStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Color = LightStore.Models.Color;

namespace LightStore.UnitTest.DataAccess
{
    [TestClass]
    public class ProductDAOTest
    {
        [TestInitialize]
        [TestCleanup]
        public void CleanUp()
        {
//            TestUtil.ClearTable("ProductAdditionalProperties");
            TestUtil.ClearTable("Products");
            TestUtil.ClearTable("Categories");

            Category category = new Category() { Code = 201, Name = "Electronic" };
            CategoryDAO.Save(category);

            Category subCategory = new Category() { Code = 202, Name = "LabTop", ParentCode = 201 };
            CategoryDAO.Save(subCategory);
        }

        [TestMethod]
        public void Test()
        {
            List<string> l = new List<string>();
            l.Add("T1");
            l.Add("T1");
            l.Add("T2");

            int c = 0;
            List<IGrouping<string, string>> l2 = l.GroupBy(s => s).ToList();
        }

        [TestMethod]
        public void Get()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 1, Title = "Lenovo 1020", Category = subCategory };
            product.Pic = File.ReadAllBytes(@"..\..\Watch.jpg");
            List<Color> colors = new List<Color>();
            colors.Add(Color.Blue);
            colors.Add(Color.Red);
            product.Colors = colors;
            product.Price = 12;
            product.IsRecommended = true;
            ProductDAO.Save(product);

            Assert.AreEqual(1, TestUtil.GetRecordsCount("Products"));
            product = ProductDAO.Get(1);
            Assert.AreEqual(1, product.Code);
            Assert.AreEqual("Lenovo 1020", product.Title);
            Assert.IsNotNull(product.Colors);
            Assert.AreEqual(2, product.Colors.Count);
            Assert.IsNotNull(product.Pic);
            Assert.AreEqual(12, product.Price);
            Assert.IsTrue(product.IsRecommended);

            product.Description = "Descriptio of Lenovo";
            product.Price = 13;
            product.Colors.Add(Color.Brown);
            ProductDAO.Save(product);

            product = ProductDAO.Get(1);
            Assert.AreEqual(1, product.Code);
            Assert.AreEqual("Lenovo 1020", product.Title);
            Assert.AreEqual("Descriptio of Lenovo", product.Description);
            Assert.IsNotNull(product.Colors);
            Assert.AreEqual(3, product.Colors.Count);
            Assert.AreEqual(13, product.Price);
        }


        [TestMethod]
        public void SaveProductWithoutPic()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 1, Title = "P1", Category = subCategory};
            ProductDAO.Save(product);

            product = ProductDAO.Get(1);
            Assert.IsNotNull(product);
            Assert.IsNull(product.Pic);
        }

        [TestMethod]
        public void Search()
        {
            Category subCategory = CategoryDAO.Get(202);
            ProductDAO.Save(new Product() {Code =  1, Title = "Lenovo", Category = subCategory, Price = 1200, IsRecommended = true});

            ProductSearch productSearch = new ProductSearch() { CategoryCode = 202, MinPrice = 1000, MaxPrice = 1500, Title = "Len", OnlyRecommended = true};
            List<Product> products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        public void SearchByDiscountedPrice()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 1, Title = "Asus", Category = subCategory, Price = 1000};
            ProductDAO.Save(product);
            ProductSearch productSearch = new ProductSearch() { CategoryCode = 202, MinPrice = 800, MaxPrice = 900 };
            List<Product> products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            product.Discount = 10;
            ProductDAO.Save(product);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            product.Discount = null;
            ProductDAO.Save(product);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            subCategory.Discount = 10;
            CategoryDAO.Save(subCategory);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            subCategory.Discount = null;
            CategoryDAO.Save(subCategory);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            Category category = CategoryDAO.Get(subCategory.ParentCode.Value);
            category.Discount = 10;
            CategoryDAO.Save(category);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod]
        public void SearchByHasDiscount()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 1, Title = "Asus", Category = subCategory, Price = 1000 };
            ProductDAO.Save(product);
            ProductSearch productSearch = new ProductSearch() { CategoryCode = 202, OnlyDiscounted = true};
            List<Product> products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            product.Discount = 10;
            ProductDAO.Save(product);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            product.Discount = null;
            ProductDAO.Save(product);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            subCategory.Discount = 10;
            CategoryDAO.Save(subCategory);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            subCategory.Discount = null;
            CategoryDAO.Save(subCategory);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            Category category = CategoryDAO.Get(subCategory.ParentCode.Value);
            category.Discount = 10;
            CategoryDAO.Save(category);
            products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod]
        public void SearchByParentCategortyCode()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 1, Title = "Asus", Category = subCategory, Price = 1000 };
            ProductDAO.Save(product);
            ProductSearch productSearch = new ProductSearch() { ParentCategoryCode = subCategory.ParentCode};
            List<Product> products = ProductDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod]
        public void Discount()
        {
            Category category = new Category() { Code = 301, Name = "House Instruments"};
            CategoryDAO.Save(category);

            Category subCategory = new Category() { Code = 302, Name = "Kitchen", ParentCode = 301 };
            CategoryDAO.Save(subCategory);

            Product product = new Product() {Code =  3, Title = "Teapot", Category = subCategory, Price = 1200};
            ProductDAO.Save(product);
            product = ProductDAO.Get(3);
            Assert.IsNull(product.Discount);

            subCategory.Discount = 20;
            CategoryDAO.Save(subCategory);
            product = ProductDAO.Get(3);
            Assert.AreEqual(20, product.Discount);

            category.Discount = 10;
            CategoryDAO.Save(category);
            product = ProductDAO.Get(3);
            Assert.AreEqual(20, product.Discount);

            subCategory.Discount = null;
            CategoryDAO.Save(subCategory);
            product = ProductDAO.Get(3);
            Assert.AreEqual(10, product.Discount);

            product.Discount = 50;
            ProductDAO.Save(product);
            product = ProductDAO.Get(3);
            Assert.AreEqual(50, product.Discount);
        }
    }
}
