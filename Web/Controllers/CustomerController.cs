using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Web.Mvc;
using BookStore.DataAccess;
using BookStore.Models;
using BookStore.Web.Security;

namespace BookStore.Web.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public void SignUp(string email, string password, string repassword)
        {
            Session[DataKeys.Error] = null;

            if (email == null || email.Trim() == "")
                Session[DataKeys.Error] = "Email field is required.";
            else if (password == null || password.Trim() == "")
                Session[DataKeys.Error] = "Password field is required.";
            else if (repassword == null || repassword.Trim() == "")
                Session[DataKeys.Error] = "Re-Password field is required.";
            else if (password != repassword)
                Session[DataKeys.Error] = "Mismatch password.";
            else if (CustomerDAO.IsExist(email))
                Session[DataKeys.Error] = "Current email is exists.";
            else
            {
                Customer customer = new Customer();
                customer.Email = email;
                customer.Password = password;

                CustomerDAO.Save(customer);
                Session[DataKeys.User] = customer;
                Session[DataKeys.RefererPage] = Request.UrlReferrer;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public void Login(string email, string password)
        {
            Session[DataKeys.Error] = null;

            if (email == null || email.Trim() == "")
                Session[DataKeys.Error] = "Email field is required.";
            else if (password == null || password.Trim() == "")
                Session[DataKeys.Error] = "Password field is required.";
            else
            {
                Customer customer = CustomerDAO.Get(email, password);

                if (customer != null)
                {
                    Session[DataKeys.User] = customer;
                    Session[DataKeys.RefererPage] = Request.UrlReferrer;
                }
                else
                    Session[DataKeys.Error] = "Incorrect email or password.";
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["User"] = null;
            string refererUrl = getRefererUrl();
            string refererPageName = CommonUtils.GetPageName(refererUrl);
            if (refererPageName != "Login".ToLower() && refererPageName != "ShopList".ToLower())
                return Redirect(getRefererUrl());
            else
                return Redirect("/Book/ListBooks");
        }

        [HttpGet]
        public ActionResult ShopList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShopList(string dummyParameter)
        {
            List<int> bookCodes = (List<int>)Session[DataKeys.ShopItems];
            string customerEmail = null;
            if (Session[DataKeys.User] != null)
                customerEmail = ((Customer) Session[DataKeys.User]).Email;

            Order order = CommonUtils.GetOrder(customerEmail, bookCodes);
            OrderDAO.Save(order);
            TempData[DataKeys.Order] = order;
            Session[DataKeys.ShopItems] = null;
            Session[DataKeys.ShopItemsCount] = null;

            sendReceiptToCustomerEmail(order);

            return Redirect("ShowReceipt");
        }

        [HttpGet]
        public ActionResult ShowReceipt()
        {
            Order order = (Order) TempData[DataKeys.Order];

            return View(order);
        }

        private string getRefererUrl()
        {
            if (Session[DataKeys.RefererPage] != null)
                return Session[DataKeys.RefererPage].ToString();
            else if (ConfigurationManager.AppSettings[DataKeys.RefererPage] != null)
                return ConfigurationManager.AppSettings[DataKeys.RefererPage];
            else
                return "/Book/ListBooks";
        }

        private void sendReceiptToCustomerEmail(Order order)
        {
            try
            {
                Customer currentUser = AuthorizeHelper.GetCurrentUser();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("BookStore.SE@gmail.com");
                mail.To.Add(currentUser.Email);


                mail.Subject = "Book Store Receipt - " + order.ReceiptNumber;
                mail.Body = string.Format("Book Store Order.{0}Order ReceiptNumber: {1}{0}Amount:{2}{0}Date: {3}-{4}{0}Time: {5}",
                    Environment.NewLine, order.ReceiptNumber, order.SaleAmount, order.Date.ToShortDateString(), order.Date.DayOfWeek, order.Time);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("BookStore.SE@gmail.com", "bs123456bs");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                //We should handel the exception in future.
            }
        }
    }
}
