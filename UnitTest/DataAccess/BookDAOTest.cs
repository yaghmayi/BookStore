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

namespace LightStore.UnitTest.DataAccess
{
    [TestClass]
    public class BookDAOTest
    {
        [TestInitialize]
        [TestCleanup]
        public void CleanUp()
        {
            TestUtil.ClearTable("Products");
        }

        [TestMethod]
        public void Get()
        {
            Book product = new Book() { Code = 1, Title = "Lenovo 1020"};
            product.Pic = File.ReadAllBytes(@"..\..\Watch.jpg");
            product.Price = 12;
            product.IsRecommended = true;
            BookDAO.Save(product);

            Assert.AreEqual(1, TestUtil.GetRecordsCount("Products"));
            product = BookDAO.Get(1);
            Assert.AreEqual(1, product.Code);
            Assert.AreEqual("Lenovo 1020", product.Title);
            Assert.IsNotNull(product.Pic);
            Assert.AreEqual(12, product.Price);
            Assert.IsTrue(product.IsRecommended);

            product.Description = "Descriptio of Lenovo";
            product.Price = 13;
            BookDAO.Save(product);

            product = BookDAO.Get(1);
            Assert.AreEqual(1, product.Code);
            Assert.AreEqual("Lenovo 1020", product.Title);
            Assert.AreEqual("Descriptio of Lenovo", product.Description);
            Assert.AreEqual(13, product.Price);
        }


        [TestMethod]
        public void SaveProductWithoutPic()
        {
            Book product = new Book() { Code = 1, Title = "P1"};
            BookDAO.Save(product);

            product = BookDAO.Get(1);
            Assert.IsNotNull(product);
            Assert.IsNull(product.Pic);
        }

        [TestMethod]
        public void Search()
        {
            BookDAO.Save(new Book() {Code =  1, Title = "Lenovo"});

            BookSearch productSearch = new BookSearch() {Title = "Len", OnlyRecommended = true};
            List<Book> products = BookDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count);
        }

        

        [TestMethod]
        public void SearchByHasDiscount()
        {
            Book product = new Book() { Code = 1, Title = "Asus", Price = 1000 };
            BookDAO.Save(product);
            BookSearch productSearch = new BookSearch() { OnlyDiscounted = true};
            List<Book> products = BookDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            product.Discount = 10;
            BookDAO.Save(product);
            products = BookDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            product.Discount = null;
            BookDAO.Save(product);
            products = BookDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            products = BookDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());

            products = BookDAO.Search(productSearch);
            Assert.AreEqual(0, products.Count());

            products = BookDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod]
        public void SearchByParentCategortyCode()
        {
            Book product = new Book() { Code = 1, Title = "Asus", Price = 1000 };
            BookDAO.Save(product);
            BookSearch productSearch = new BookSearch() { Title = "Asu"};
            List<Book> products = BookDAO.Search(productSearch);
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod]
        public void Discount()
        {
            Book product = new Book() {Code =  3, Title = "Teapot", Price = 1200};
            BookDAO.Save(product);
            product = BookDAO.Get(3);
            Assert.IsNull(product.Discount);

            product = BookDAO.Get(3);
            Assert.AreEqual(20, product.Discount);

            product = BookDAO.Get(3);
            Assert.AreEqual(20, product.Discount);

            product = BookDAO.Get(3);
            Assert.AreEqual(10, product.Discount);

            product.Discount = 50;
            BookDAO.Save(product);
            product = BookDAO.Get(3);
            Assert.AreEqual(50, product.Discount);
        }
    }
}
