using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using BookStore.Models;

namespace BookStore.DataAccess
{
    public static class BookDAO
    {
        private static DAOHelper<Book> daoHelper  = new DAOHelper<Book>("Books", "Code");

        private static string imageFolder
        {
            get
            {
                string dataSourceFolder = ConfigurationManager.AppSettings.Get("DataSourceFolder");

                return AppDomain.CurrentDomain.BaseDirectory + "\\" + dataSourceFolder + "\\Images";
            }
        }

        public static List<Book> GetAll()
        {
            List<Book> books = daoHelper.GetAll();

            foreach  (Book book in books)
            {
                string imageFilePath = imageFolder + "//" + book.Code + ".jpg";
                if (File.Exists(imageFilePath))
                    book.Pic = File.ReadAllBytes(imageFilePath);
                else
                    book.Pic = File.ReadAllBytes(imageFolder + "//defaultImage.jpg");
            }

            return books;
        }

        public static Book Get(int code)
        {
            List<Book> books = GetAll();
            Book book = books.Find(bk => bk.Code == code);

            return book;
        }

        public static void CheckOut(int code)
        {
            if (IsExist(code))
            {
                Book book = Get(code);
                if (book.InStock > 0)
                {
                    book.InStock = book.InStock - 1;
                    daoHelper.Save(book);
                }
            }
        }

        public static void CheckIn(int code, int boughtItems)
        {
            if (IsExist(code))
            {
                Book book = Get(code);
                book.InStock = book.InStock + boughtItems;

                daoHelper.Save(book);
            }
        }


        private static bool IsExist(int code)
        {
            Book book = Get(code);

            return book != null;
        }

        public static List<Book> Search(string searchTrem)
        {
            List<Book> books = GetAll();

            if (!string.IsNullOrEmpty(searchTrem))
            {
                string txt = searchTrem.ToLower().Trim();

                books = books.FindAll(bk => !string.IsNullOrEmpty(bk.Title) &&
                                            (bk.Title.ToLower().Contains(txt) || bk.Author.ToLower().Contains(txt)));
            }

            return books;
        }
    }
}