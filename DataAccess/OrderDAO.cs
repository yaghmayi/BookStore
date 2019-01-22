using System.Collections.Generic;
using BookStore.Models;

namespace BookStore.DataAccess
{
    public class OrderDAO
    {
        private static DAOHelper<Order> daoHelper = new DAOHelper<Order>("Orders", "ReceiptNumber");

        public static void Save(Order order)
        {
            daoHelper.Save(order);
        }

        public static List<Order> GetAll()
        {
            return daoHelper.GetAll();
        }
    }
}
