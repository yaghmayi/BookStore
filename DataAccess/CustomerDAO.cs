using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BookStore.Models;

namespace BookStore.DataAccess
{
    public static class CustomerDAO
    {
        private static DAOHelper<Customer> daoHelper = new DAOHelper<Customer>("Customers", "Email");

        public static bool Save(Customer customer)
        {
            if (!IsExist(customer.Email))
            {
                daoHelper.Save(customer);

                return true;
            }
            else
                return false;
        }

        public static Customer Get(string email, string password)
        {
            Customer customer = GetAll().Find(c => c.Email.ToLower() == email.ToLower() && c.Password == password);
            return customer;
        }

        public static Customer Get(string email)
        {
            Customer customer =  GetAll().Find(c => c.Email.ToLower() == email.ToLower());
            return customer;
        }

        public static bool IsExist(string email)
        {
            return Get(email) != null;
        }

        public static List<Customer> GetAll()
        {
            List<Customer> customers = daoHelper.GetAll();

            return customers;
        }
    }
}