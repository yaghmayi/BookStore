using System;

namespace BookStore.Models
{
    public class Receipt
    {
        public int Number { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime Date { get; set; }

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
