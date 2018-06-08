using System.Collections.Generic;
using System.Web;
using HtmlHelper = System.Web.WebPages.Html.HtmlHelper;
using BookStore.DataAccess;
using BookStore.Models;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace BookStore.Web
{
    public static class  BookStotreHtmHelper
    {
        public static HtmlString DisplayLabel(object fieldValue)
        {
            string html = string.Format(@"<label style=""margin-bottom: 2px"">{0}<label>", fieldValue);

            return new HtmlString(html);
        }


        public static string NumberToMoneyFormat(decimal? number)
        {
            if (number == null)
                return "-";
            else if (number > 0)
            {
                var splittedNumber = number.ToString().Split('.');

                if (splittedNumber.Length == 1)
                    return string.Format("{0:#,0}", number);
                else
                {
                    int realSideNo = splittedNumber[1].Length;
                    string realSide = "";
                    string format = "{0:#,0."; 
                    format += realSide.PadLeft(realSideNo, '0') + "}";

                    return string.Format(format, number);
                }
            }

            return "0";
        }

        public static HtmlString DisplayPrice(decimal? price, int? discount)
        {
            string html = NumberToMoneyFormat(price);
            if (discount != null && discount > 0)
            {
                decimal? salePrice = decimal.Round(price.Value - (price.Value * discount.Value / 100));
                html = string.Format(@"<strike>{0}</strike> {1}", NumberToMoneyFormat(price), NumberToMoneyFormat(salePrice));
            }

            html = string.Format(@"<span class=""label label-primary"">${0}</span>", html);

            return new HtmlString(html);
        }

        public static HtmlString DisplayPrice(decimal? price, decimal? salePrice)
        {
            string html = string.Format(@"<strike>{0}</strike> {1}", NumberToMoneyFormat(price), NumberToMoneyFormat(salePrice));
            html = string.Format(@"<span class=""label label-primary"">${0}</span>", html);

            return new HtmlString(html);
        }

        public static HtmlString DisplayMessage(string message)
        {
            return new HtmlString(message);
        }

        public static string Contact(string str1, string str2)
        {
            return str1 + str2;
        }
    }
}