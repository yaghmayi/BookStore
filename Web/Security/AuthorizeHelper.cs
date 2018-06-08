using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.Web.Security
{
    public static class AuthorizeHelper
    {
        public static bool IsCurrentUserAdmin()
        {
            if (!IsAuthorizationEnable())
                return true;
            else if (HttpContext.Current.Session[DataKeys.User] != null)
            {
                Customer customer = (Customer) HttpContext.Current.Session[DataKeys.User];
                if (customer.Email.ToLower() == "admin")
                    return true;
            }

            return false;
        }

        public static bool IsAuthorizationEnable()
        {
            if (ConfigurationManager.AppSettings["Authorization"] != null)
                return bool.Parse(ConfigurationManager.AppSettings["Authorization"]);
            else
                return true;
        }

        public static Customer GetCurrentUser()
        {
            Customer customer = null;
            if (HttpContext.Current.Session[DataKeys.User] != null)
                customer = (Customer) HttpContext.Current.Session[DataKeys.User];
            
            return customer;
        }
    }
}