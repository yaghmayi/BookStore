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
    public class CustomerDAOTest
    {
        [TestMethod]
        public void Save()
        {
            List<Customer> customers = CustomerDAO.GetAll();
            Assert.AreEqual(0, customers.Count);


            Customer customer = new Customer();
            customer.Email = "customerEmail@gmail.com";
            customer.Password = "TT";
            CustomerDAO.Save(customer);

            customers = CustomerDAO.GetAll();
            Assert.AreEqual(1, customers.Count);

            

            customer = new Customer();
            customer.Email = "fooEmail@gmail.com";
            customer.Password = "FooPass";
            CustomerDAO.Save(customer);

            customers = CustomerDAO.GetAll();
            Assert.AreEqual(2, customers.Count);



            CustomerDAO.Save(customer);

            customers = CustomerDAO.GetAll();
            Assert.AreEqual(2, customers.Count);
        }
    }
}
