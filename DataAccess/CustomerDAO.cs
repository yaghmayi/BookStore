using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LightStore.Models;

namespace LightStore.DataAccess
{
    public static class CustomerDAO
    {
        public static string tableName = "Customers";

        public static string keyName = "Email";

        public static bool Save(Customer customer)
        {
            if (IsExist(customer.Email))
                return false;
            else
            {
                SqlConnection conn = BaseDAO.GetSqlConnection();
                string sql = string.Format("Insert into Customers (Email) Values ('{0}')", customer.Email);
                SqlCommand cm = new SqlCommand(sql, conn);
                conn.Open();
                cm.ExecuteNonQuery();
                conn.Close();

                return true;
            }
        }

        public static Customer Get(string email)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            string sql = string.Format("select * from Customers where Email='{0}'", email);
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cm.ExecuteReader();
            Customer customer = null;
            if (dr.Read())
            {
                customer = new Customer();
                customer.Email = dr["Email"].ToString();
            }
            conn.Close();

            return customer;

        }

        public static bool IsExist(string email)
        {
            return BaseDAO.IsExist(tableName, keyName, email);
        }
    }
}