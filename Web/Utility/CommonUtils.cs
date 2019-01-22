using System;
using System.Collections.Generic;
using BookStore.DataAccess;
using BookStore.Models;

namespace BookStore.Web
{
    public class CommonUtils
    {
        public static Order GetOrder(string customerEmail, List<int> bookCodes)
        {
            DateTime today = DateTime.Now;
            decimal totalAmount = 0;
            decimal saleAmount = 0;
            foreach (int productCode in bookCodes)
            {
                Book product = BookDAO.Get(productCode);
                totalAmount += product.Price;
                saleAmount += product.SalePrice;
            }

            Order order = new Order();
            order.CustomerEmail = customerEmail;
            order.TotalAmount = totalAmount;
            order.SaleAmount = saleAmount;
            order.Date = today;
            order.ReceiptNumber = Math.Abs(today.GetHashCode());

            foreach (int bookCode in bookCodes)
            {
                OrderItem orderItem = order.OrderItems.Find(item => item.BookCode == bookCode);
                if (orderItem != null)
                    orderItem.Quantity++;
                else
                {
                    Book book = BookDAO.Get(bookCode);
                    order.addOrderItem(book.Code, book.Title, 1);
                }
            }
                

            return order;

        }
    }
}