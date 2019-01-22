using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.UnitTest
{
    [TestClass]
    public class OrderDAOTest
    {
        [TestMethod]
        public void Save()
        {
            List<Order> orders = OrderDAO.GetAll();
            Assert.AreEqual(0, orders.Count);

            Order order = new Order();
            order.ReceiptNumber = 1001;
            order.TotalAmount = 200;
            order.addOrderItem(10, "Harry Pater", 1); 
            order.addOrderItem(20, "English in usagae", 2);
            OrderDAO.Save(order);

            orders = OrderDAO.GetAll();
            Assert.AreEqual(1, orders.Count);

            order = orders[0];
            Assert.AreEqual(1001, order.ReceiptNumber);
            Assert.AreEqual(2, order.OrderItems.Count);
            OrderItem orderItem = order.OrderItems[0];
            Assert.IsNotNull(orderItem);
            Assert.AreEqual(10, orderItem.BookCode);
            Assert.AreEqual(1, orderItem.Quantity);
            orderItem = order.OrderItems[1];
            Assert.IsNotNull(orderItem);
            Assert.AreEqual(20, orderItem.BookCode);
            Assert.AreEqual(2, orderItem.Quantity);

            order = new Order();
            order.ReceiptNumber = 1002;
            order.TotalAmount = 300;
            OrderDAO.Save(order);

            orders = OrderDAO.GetAll();
            Assert.AreEqual(2, orders.Count);

            OrderDAO.Save(order);
            Assert.AreEqual(2, orders.Count);
        }
    }
}
