using BookStore.Models;

namespace BookStore.DataAccess
{
    public static class CustomerDAO
    {
        public static bool Save(Customer customer)
        {
            //This method should register a customer using his/her email.
            //Currently, This is a fake method.

            return true;
        }

        public static Customer Get(string email)
        {
            //This method should get a customer by email.
            //Currently, This is a fake method.

            Customer customer = new Customer();
            customer.Email = email;

            return customer;

        }

        public static bool IsExist(string email)
        {
            //This method should return if the desired eamil has been registered as a user name.
            //Currently, This is a fake method.

            return false;
        }
    }
}