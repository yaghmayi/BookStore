using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
using LightStore.DataAccess;
using LightStore.Models;
using LightStore.Web.Controllers;
using HtmlHelper = System.Web.WebPages.Html.HtmlHelper;
using System.Web.Mvc.Html;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace LightStore.Web
{
    public static class  LightStotreHtmHelper
    {
        public static HtmlString DisplayLabel(object fieldValue)
        {
//            string html = string.Format(@"<input value=""{0}"" type=""text"" disabled=""disabled"" style=""margin-bottom: 2px"" />", fieldValue);
            string html = string.Format(@"<label style=""margin-bottom: 2px"">{0}<label>", fieldValue);

            return new HtmlString(html);
        }

        public static HtmlString DisplaySize(int? length, int? width, int? height)
        {
            string sizeStr = "";
            if (HasSizeValue(length))
                sizeStr += length + " * ";
            if (HasSizeValue(width))
                sizeStr += width + " * ";
            if (HasSizeValue(height))
                sizeStr += height + " * ";
            if (sizeStr != "")
                sizeStr = sizeStr.Substring(0, sizeStr.Length - " * ".Length);
            else
                sizeStr = "-";


            string html = string.Format(@"<label style=""margin-bottom: 2px"">{0} (Size)<label>", sizeStr);

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
                    string format = "{0:#,0."; //+realSide.PadLeft(realSideNo, '0') + "}";
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

        public static HtmlString DisplayPrice(List<int> productCodes)
        {
            decimal totalPrice = 0;
            decimal totalSalePrice = 0;
            bool hasDiscount = false;
            foreach (int productCode in productCodes)
            {
                Book product = BookDAO.Get(productCode);
                totalPrice += product.Price;
                totalSalePrice += product.SalePrice;

                if (product.HasDiscount)
                    hasDiscount = true;
            }


            string html = NumberToMoneyFormat(totalSalePrice);
            if (hasDiscount)
                html = string.Format(@"<strike>{0}</strike> {1}", NumberToMoneyFormat(totalSalePrice), NumberToMoneyFormat(totalSalePrice));
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

        private static bool HasSizeValue(int? val)
        {
            return val != null && val > 0;
        }


    }
}