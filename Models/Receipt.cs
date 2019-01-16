using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public class Receipt
    {
        public int Number { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime Date { get; set; }
        
        public List<Book> Books = new List<Book>();

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
    }
}
