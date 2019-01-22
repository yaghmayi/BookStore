using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookStore.Models
{
    public class OrderItem
    {
        private int bookCode;
        private string bookTitle;
        private int quantity;

        public OrderItem()
        {
        }

        public OrderItem(int bookCode, string bookTitle, int quantity) : this()
        {
            this.bookCode = bookCode;
            this.bookTitle = bookTitle;
            this.quantity = quantity;
        }

        [XmlAttribute]
        public int BookCode { get => bookCode; set => bookCode = value; }

        [XmlAttribute]
        public string BookTitle { get => bookTitle; set => bookTitle = value; }

        [XmlAttribute]
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
