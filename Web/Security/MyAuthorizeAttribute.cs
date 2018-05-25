using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LightStore.Models;
using LightStore.Web.Security;

namespace LightStore.Web
{
    public class MyAthorizeAttribute : AuthorizeAttribute
    {
        private string role = null;


        public MyAthorizeAttribute()
        {
        }

        public MyAthorizeAttribute(string role)
        {
            this.role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!AuthorizeHelper.IsAuthorizationEnable() || this.role == null)
                return true;
            else
            {
                Customer customer = (Customer) HttpContext.Current.Session["User"];
                if (customer == null || customer.Email.ToLower() != this.role.ToLower())
                    return false;
                else
                    return true;
            }
        }
    }
}