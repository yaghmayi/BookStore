using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookStore.Models
{
    public class Order
    {
        private List<OrderItem> orderItems = new List<OrderItem>();

        [XmlAttribute]
        public int ReceiptNumber { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime Date { get; set; }

        public List<OrderItem> OrderItems
        {
            get { return orderItems; }
        }

        public string Time
        {
            get
            {
                return Date.Hour + ":" + Date.Minute;
            }
        }

        public bool HasDiscont
        {
            get { return SaleAmount < TotalAmount;  }
        }

        public void addOrderItem(int bookCode, string bookTitle, int quantity)
        {
            OrderItem orderItem = new OrderItem(bookCode, bookTitle, quantity);
            this.orderItems.Add(orderItem);
        }
    }
}
