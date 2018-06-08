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
        public void SignUp(string email, string password, string repassword)
        {
            if (password == repassword && !CustomerDAO.IsExist(email))
            {
                Customer customer = new Customer();
                customer.Email = email;
                customer.Password = password;

                CustomerDAO.Save(customer);
                Session[DataKeys.User] = customer;
                Session[DataKeys.RefererPage] = Request.UrlReferrer;
            }
        }

        public void Login(string email, string password)
        {
            Customer customer = CustomerDAO.Get(email);
            
            if (customer != null)
            {
                Session[DataKeys.User] = customer;
                Session[DataKeys.RefererPage] = Request.UrlReferrer;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["User"] = null;
            return Redirect(GetRefererPage());
        }

        [HttpGet]
        public ActionResult ShopList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShopList(string dummyParameter)
        {
            List<int> productCodes = (List<int>)Session[DataKeys.ShopItems];
            Receipt receipt = CommonUtils.GetReceipt(productCodes);
            TempData[DataKeys.Receipt] = receipt;
            Session[DataKeys.ShopItems] = null;
            Session[DataKeys.ShopItemsCount] = null;

            return Redirect("ShowReceipt");
        }

        private string GetRefererPage()
        {
            if (Session[DataKeys.RefererPage] != null)
                return Session[DataKeys.RefererPage].ToString();
            else if (ConfigurationManager.AppSettings[DataKeys.RefererPage] != null)
                return ConfigurationManager.AppSettings[DataKeys.RefererPage];
            else
                return "/Product/ListBooks";
        }


        [HttpGet]
        public ActionResult ShowReceipt()
        {
            Receipt receipt = (Receipt) TempData[DataKeys.Receipt];
            Customer currentUser = AuthorizeHelper.GetCurrentUser();
            if (currentUser != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("fooemail6@gmail.com");
                mail.To.Add(currentUser.Email);


                mail.Subject = "Book Store Receipt Number";
                mail.Body = string.Format("Book Store Receipt.{0}Receipt Number: {1}{0}Amount:{2}{0}Date: {3}-{4}{0}Time: {5}",
                                          Environment.NewLine, receipt.Number, receipt.SaleAmount, receipt.Date.ToShortDateString(), receipt.Date.DayOfWeek, receipt.Time);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("fooemail6@gmail.com", "fooemail1234");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }

            return View(receipt);
        }


    }
}
