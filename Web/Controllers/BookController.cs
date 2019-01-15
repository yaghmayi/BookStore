using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookStore.DataAccess;
using BookStore.Models;
using BookStore.Services;
using BookStore.Web.Controllers.Base;

namespace BookStore.Web.Controllers
{
    public partial class BookController : Controller
    {
        [HttpGet]
        public void AddShopItem(int id)
        {
            if (Session[DataKeys.ShopItems] == null)
                Session[DataKeys.ShopItems] = new List<int>();
            if (Session[DataKeys.ShopItemsCount] == null)
                Session[DataKeys.ShopItemsCount] = 0;

            List<int> shopItemCodes = (List<int>)Session[DataKeys.ShopItems];
            BookDAO.CheckOut(id);
            shopItemCodes.Add(id);

            int count = (int)Session[DataKeys.ShopItemsCount];
            Session[DataKeys.ShopItemsCount] = count + 1;
        }

        [HttpGet]
        public void DeleteShopItem(int id)
        {
            if (Session[DataKeys.ShopItems] == null)
                Session[DataKeys.ShopItems] = new List<int>();
            if (Session[DataKeys.ShopItemsCount] == null)
                Session[DataKeys.ShopItemsCount] = 0;

            List<int> shopItemCodes = (List<int>)Session[DataKeys.ShopItems];
            int boughtItems = shopItemCodes.FindAll(bCode => bCode == id).Count;
            BookDAO.CheckIn(id, boughtItems);
            shopItemCodes.RemoveAll(bCode => bCode == id);

            int count = (int)Session[DataKeys.ShopItemsCount];
            Session[DataKeys.ShopItemsCount] = count - boughtItems;
        }

        [HttpGet]
        public ActionResult ListBooks(String searchTerm)
        {
            BookService bookService = new BookService();
            Task<IEnumerable<IBook>> task = bookService.GetBooksAsync(searchTerm);
            ViewData[DataKeys.Books] = task.Result;

            return View();

        }

        public ActionResult ShowBook(int id)
        {
            Book book = BookDAO.Get(id);
            return View(book);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhoto(int id)
        {
            Book book = BookDAO.Get(id);
            return new ImageResult(book.Pic);
        }

        public int GetBookStock(int id)
        {
            Book book = BookDAO.Get(id);
            if (book != null)
                return book.InStock;
            else
                return 0;
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
    }
}
