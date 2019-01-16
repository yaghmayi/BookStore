using System.Collections.Generic;
using BookStore.Models;

namespace BookStore.DataAccess
{
    public class ReceiptDAO
    {
        private static DAOHelper<Receipt> daoHelper = new DAOHelper<Receipt>("Receipts", "Number");

        public static void Save(Receipt receipt)
        {
            daoHelper.Save(receipt);
        }

        public static List<Receipt> GetAll()
        {
            return daoHelper.GetAll();
        }
    }
}
