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
    public class ReceiptDAOTest
    {
        [TestMethod]
        public void Save()
        {
            List<Receipt> receipts = ReceiptDAO.GetAll();
            Assert.AreEqual(0, receipts.Count);

            Receipt receipt = new Receipt();
            receipt.Number = 1001;
            receipt.TotalAmount = 200;
            ReceiptDAO.Save(receipt);

            receipts = ReceiptDAO.GetAll();
            Assert.AreEqual(1, receipts.Count);

            receipt = new Receipt();
            receipt.Number = 1002;
            receipt.TotalAmount = 300;
            ReceiptDAO.Save(receipt);

            receipts = ReceiptDAO.GetAll();
            Assert.AreEqual(2, receipts.Count);

            ReceiptDAO.Save(receipt);
            Assert.AreEqual(2, receipts.Count);
        }
    }
}
