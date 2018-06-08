using System.Collections.Generic;
using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.UnitTest.DataAccess
{
    [TestClass]
    public class BookDAOTest
    {
        [TestInitialize]
        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void GetAll()
        {
            List<Book> books = BookDAO.GetAll();
            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Count);

            Book book = books[0];
            Assert.IsNotNull(book);
            Assert.AreEqual(101, book.Code);
            Assert.AreEqual("Book1", book.Title);
            Assert.IsNotNull(book.Pic);
            Assert.IsTrue(book.HasDiscount);

            book = books[1];
            Assert.IsNotNull(book);
            Assert.AreEqual(102, book.Code);
            Assert.AreEqual("Book2", book.Title);
            Assert.IsNotNull(book.Pic);
            Assert.IsFalse(book.HasDiscount);
        }

        [TestMethod]
        public void CheckInOut()
        {
            Book book = BookDAO.Get(101);
            Assert.IsNotNull(book);
            Assert.AreEqual(2, book.InStock);

            BookDAO.CheckOut(101);
            book = BookDAO.Get(101);
            Assert.IsNotNull(book);
            Assert.AreEqual(1, book.InStock);

            BookDAO.CheckOut(101);
            book = BookDAO.Get(101);
            Assert.IsNotNull(book);
            Assert.AreEqual(0, book.InStock);

            BookDAO.CheckOut(101);
            book = BookDAO.Get(101);
            Assert.IsNotNull(book);
            Assert.AreEqual(0, book.InStock);


            BookDAO.CheckIn(101, 2);
            book = BookDAO.Get(101);
            Assert.IsNotNull(book);
            Assert.AreEqual(2, book.InStock);
        }
    }
}
