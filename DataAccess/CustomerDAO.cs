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
        private static string imageFolder
        {
            get
            {
                return DAOHelper.dataFolder + "\\Images";
            }
        }

        private static string dataFilePath
        {
            get
            {
                return DAOHelper.dataFolder + "\\Customers.xml";
            }
        }

        public static bool Save(Customer customer)
        {
            if (!IsExist(customer.Email))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(dataFilePath);

                XmlElement customersElement = (XmlElement) doc.GetElementsByTagName("Customers").Item(0);
                XmlElement customerElement = doc.CreateElement("Customer");
                XmlElement emailElement = doc.CreateElement("Email");
                emailElement.InnerText = customer.Email;
                customerElement.AppendChild(emailElement);

                XmlElement passwordElement = doc.CreateElement("Password");
                passwordElement.InnerText = customer.Password;
                customerElement.AppendChild(passwordElement);

                customersElement.AppendChild(customerElement);

                doc.Save(dataFilePath);

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
            XmlSerializer desSerializer = new XmlSerializer(typeof(List<Customer>), new XmlRootAttribute("Customers"));
            StreamReader xmlReader = new StreamReader(dataFilePath);
            List<Customer> customers = (List<Customer>)desSerializer.Deserialize(xmlReader);

            xmlReader.Close();

            return customers;
        }
    }
}