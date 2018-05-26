using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using LightStore.Models;

namespace LightStore.DataAccess
{
    public static class BookDAO
    {
        public static Book Get(int code)
        {
            return null;
            //string sql = selectQuery + string.Format(" Where prd.Code='{0}'", code);
            //return LoadProducts(sql).FirstOrDefault();
        }

        public static byte[] GetPic(int productCode)
        {
            Book product = Get(productCode);
            if (product != null)
                return product.Pic;
            else
                return null;
        }

        private static bool IsExist(int code)
        {
            return false;
            //return BaseDAO.IsExist(tableName, keyName, code.ToString());
        }

        public static List<Book> Search(BookSearch bookSearch)
        {
            return null;
            //string condition = "";
            //if (!string.IsNullOrEmpty(bookSearch.Title))
            //    condition += string.Format("prd.Title Like '%{0}%' And ", bookSearch.Title);
            //if (bookSearch.Author != null)
            //    condition += string.Format("prd.Author Like '%{0}%' And ", bookSearch.Author);
            //if (bookSearch.OnlyDiscounted)
            //    condition += "HasDiscount = 1 And ";
            //if (bookSearch.OnlyRecommended)
            //    condition += "IsRecommended = 1 And ";

            //if (condition != "")
            //{
            //    condition = condition.Substring(0, condition.Length - "And ".Length);
            //    sql += " Where " + condition;
            //}

            //return LoadProducts(sql);
        }
    }
}