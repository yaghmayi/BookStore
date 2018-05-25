using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightStore.DataAccess;
using LightStore.Models;

namespace LightStore.Web
{
    public class CommonUtils
    {
        public static Receipt GetReceipt(List<int> productCodes)
        {
            DateTime today = DateTime.Now;
            decimal totalAmount = 0;
            decimal saleAmount = 0;
            foreach (int productCode in productCodes)
            {
                Book product = BookDAO.Get(productCode);
                totalAmount += product.Price;
                saleAmount += product.SalePrice;
            }

            Receipt receipt = new Receipt();
            receipt.TotalAmount = totalAmount;
            receipt.SaleAmount = saleAmount;
            receipt.Date = today;
            receipt.Number = Math.Abs(today.GetHashCode());

            return receipt;

        }
    }
}