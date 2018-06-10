using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Services;
using BookStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.UnitTest
{
    [TestClass]
    public class BookServiceTest
    {
        [TestMethod]
        public void GetBooksAsync()
        {
            BookService bookService = new BookService();
            Task<IEnumerable<IBook>> task = bookService.GetBooksAsync("book2");
            List<Book> books = (List<Book>) task.Result;
            Assert.AreEqual(1, books.Count);

            task = bookService.GetBooksAsync("author");
            books = (List<Book>)task.Result;
            Assert.AreEqual(2, books.Count);

            task = bookService.GetBooksAsync("author10");
            books = (List<Book>)task.Result;
            Assert.AreEqual(0, books.Count);
        }
    }
}
