using System.Collections.Generic;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BookStore.Models;

namespace BookStore.DataAccess
{
    public static class BookDAO
    {
        private static string imageFolder
        {
            get
            {
                return DAOHelper.dataFolder + "//Images";
            }
        }

        private static string dataFilePath
        {
            get
            {
                return DAOHelper.dataFolder + "//Books.xml";
            }
        }

        public static List<Book> GetAll()
        {
            XmlSerializer desSerializer = new XmlSerializer(typeof(List<Book>), new XmlRootAttribute("Books"));
            StreamReader xmlReader = new StreamReader(dataFilePath);
            List<Book> books = (List<Book>) desSerializer.Deserialize(xmlReader);

            foreach  (Book book in books)
            {
                string imageFilePath = imageFolder + "//" + book.Code + ".jpg";
                if (File.Exists(imageFilePath))
                    book.Pic = File.ReadAllBytes(imageFilePath);
                else
                    book.Pic = File.ReadAllBytes(imageFolder + "//defaultImage.jpg");
            }

            xmlReader.Close();

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
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(dataFilePath);
                    XmlNode inStockNode = xmlDoc.SelectSingleNode("Books/Book[@Code=" + code + "]/InStock"); 
                    int inStock = int.Parse(inStockNode.InnerText);
                    inStockNode.InnerText = (inStock - 1).ToString();

                    xmlDoc.Save(dataFilePath);
                }
            }
        }

        public static void CheckIn(int code, int boughtItems)
        {
            if (IsExist(code))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dataFilePath);
                XmlNode inStockNode = xmlDoc.SelectSingleNode("Books/Book[@Code=" + code + "]/InStock");
                int inStock = int.Parse(inStockNode.InnerText);
                inStockNode.InnerText = (inStock + boughtItems).ToString();

                xmlDoc.Save(dataFilePath);
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