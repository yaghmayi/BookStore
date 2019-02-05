using System.Collections.Generic;
using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.UnitTest
{
    [TestClass]
    public class BookDAOTest
    {
        [TestMethod]
        public void GetAll()
        {
            List<Book> books = BookDAO.GetAll();
            Assert.IsNotNull(books);
            Assert.AreEqual(5, books.Count);

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
        public void GetAll_Pagination()
        {
            List<Book> books = BookDAO.GetAll(1, 2);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book1", books[0].Title);
            Assert.AreEqual("Book2", books[1].Title);

            books = BookDAO.GetAll(2, 2);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book3", books[0].Title);
            Assert.AreEqual("Book4", books[1].Title);

            books = BookDAO.GetAll(3, 2);
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Book5", books[0].Title);

            books = BookDAO.GetAll(4, 2);
            Assert.AreEqual(0, books.Count);

            books = BookDAO.GetAll(1, 3);
            Assert.AreEqual(3, books.Count);
            Assert.AreEqual("Book1", books[0].Title);
            Assert.AreEqual("Book2", books[1].Title);
            Assert.AreEqual("Book3", books[2].Title);

            books = BookDAO.GetAll(2, 3);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book4", books[0].Title);
            Assert.AreEqual("Book5", books[1].Title);

            books = BookDAO.GetAll(1, 5);
            Assert.AreEqual(5, books.Count);

            books = BookDAO.GetAll(2, 5);
            Assert.AreEqual(0, books.Count);

            books = BookDAO.GetAll(1, 7);
            Assert.AreEqual(5, books.Count);

            books = BookDAO.GetAll(2, 7);
            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void Search()
        {
            List<Book> books = BookDAO.Search("book");
            Assert.AreEqual(5, books.Count);

            books = BookDAO.Search("book2");
            Assert.AreEqual(1, books.Count);

            books = BookDAO.Search("author2");
            Assert.AreEqual(1, books.Count);

            books = BookDAO.Search("author10");
            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void Search_Pagination()
        {
            List<Book> books = BookDAO.Search("book", 1, 2);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book1", books[0].Title);
            Assert.AreEqual("Book2", books[1].Title);

            books = BookDAO.Search("book", 2, 2);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book3", books[0].Title);
            Assert.AreEqual("Book4", books[1].Title);

            books = BookDAO.Search("book", 3, 2);
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Book5", books[0].Title);

            books = BookDAO.Search("ThereIsNo", 1, 5);
            Assert.AreEqual(0, books.Count);
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
