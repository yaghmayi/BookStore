using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookStore.DataAccess;
using BookStore.Models;
using BookStore.Web.Controllers.Base;

namespace BookStore.Web.Controllers
{
    public partial class BookController : Controller
    {
        [HttpPost]
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

        [HttpPost]
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
        public ActionResult ListBooks(String searchTerm, int? page)
        {
            int bookItemsCountInEachPage = int.Parse(ConfigurationManager.AppSettings.Get("BookItemsCountInEachPage"));

            List<Book> bookItems = BookDAO.Search(searchTerm);
            int pageNumbers = (int) Math.Ceiling((decimal) bookItems.Count / bookItemsCountInEachPage);
            ViewData[DataKeys.BooksPageNumbers] = pageNumbers;
            Session[DataKeys.BooksPageNumbers] = pageNumbers;

            int pageNumber = page != null ? page.Value : 1;
            ViewData[DataKeys.Books] = BookDAO.Search(searchTerm, pageNumber, bookItemsCountInEachPage);
            ViewData[DataKeys.CurrentPage] = pageNumber;
            ViewData[DataKeys.SearchTerm] = searchTerm;

            return View();
        }

        [HttpGet]
        public ActionResult PageBooks(String searchTerm, int page)
        {
            int bookItemsCountInEachPage = int.Parse(ConfigurationManager.AppSettings.Get("BookItemsCountInEachPage"));

            List<Book> bookItems = BookDAO.Search(searchTerm);
            int pageNumbers = (int)Math.Ceiling((decimal)bookItems.Count / bookItemsCountInEachPage);
            ViewData[DataKeys.BooksPageNumbers] = pageNumbers;
            Session[DataKeys.BooksPageNumbers] = pageNumbers;

            ViewData[DataKeys.Books] = BookDAO.Search(searchTerm, page, bookItemsCountInEachPage);
            ViewData[DataKeys.CurrentPage] = page;
            ViewData[DataKeys.SearchTerm] = searchTerm;

            return PartialView();
        }

        [HttpGet]
        public ActionResult ShowBook(int id)
        {
            Book book = BookDAO.Get(id);
            return View(book);
        }

        [HttpGet]
        public ActionResult ShowPhoto(int id)
        {
            Book book = BookDAO.Get(id);
            return new ImageResult(book.Pic);
        }

        [HttpGet]
        public int GetBookStock(int id)
        {
            Book book = BookDAO.Get(id);
            if (book != null)
                return book.InStock;
            else
                return 0;
        }

        [HttpGet]
        public string GetSession(string id)
        {
            if (Session[id] != null)
                return Session[id].ToString();
            else
                return null;
        }

        [HttpPost]
        public void SetSession(string id, string value)
        {
            Session[id] = value;
        }
    }
}
