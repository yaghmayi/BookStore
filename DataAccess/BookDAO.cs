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
        private static string tableName = "Products";
        private static string keyName = "Code";

        private static string _selectQuery = null;
        private static string selectQuery
        {
            get
            {
                if (_selectQuery != null)
                    return _selectQuery;
                else
                {
                    Assembly assembly = Assembly.Load("DataAccess");
                    using (Stream stream = assembly.GetManifestResourceStream("DataAccess.ProductSelectQuery.txt"))
                    using (StreamReader reader = new StreamReader(stream))
                        _selectQuery = reader.ReadToEnd();

                    return _selectQuery;
                }
            }
        }

        public static void Save(Book product)
        {
            string sql = "";
            if (IsExist(product.Code))
            {
                sql = string.Format("Update Products set CategoryCode='{0}', Title='{1}', Description='{2}', Colors={3}, " +
                                    "Length={4}, Width={5}, Height={6}, Price={7}, Discount={8}, IsRecommended={9}, Pic=@Pic Where Code='{10}'",
                                     product.Title, product.Description,  
                                     product.Price, BaseDAO.GetDBValue(product.Discount), BaseDAO.GetDBValue(product.IsRecommended), product.Code);
            }
            else
            {
                sql = string.Format("Insert into Products (Code, CategoryCode, Title, Description, Colors, Length, Width, Height, Price, Discount, IsRecommended, Pic) " +
                                    "Values ('{0}','{1}', '{2}', '{3}',{4}, {5}, {6}, {7}, {8}, {9}, {10}, @Pic)",
                                    product.Code, product.Title, product.Description, 
                                    product.Price, BaseDAO.GetDBValue(product.Discount), BaseDAO.GetDBValue(product.IsRecommended));
            }

            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);

            var picParameter = new SqlParameter("@Pic", SqlDbType.Binary);
            if (product.Pic != null)
                picParameter.Value = product.Pic;
            else
                picParameter.Value = DBNull.Value;

            cm.Parameters.Add(picParameter);

            conn.Open();
            cm.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Book> GetByCategoryCode(int categoryCode)
        {
            string sql = selectQuery + string.Format(" Where prd.CategoryCode='{0}'", categoryCode);
            return LoadProducts(sql);
        }

        public static Book Get(int code)
        {
            string sql = selectQuery + string.Format(" Where prd.Code='{0}'", code);
            return LoadProducts(sql).FirstOrDefault();
        }

        public static void Delete(int code)
        {
            BaseDAO.Delete(tableName, keyName, code.ToString());
        }

        public static int GetNextCode()
        {
            return BaseDAO.GetNextID(tableName, keyName);
        }

        public static void DeleteByCategoryCode(int categoryCode)
        {
            BaseDAO.Delete(tableName, "categoryCode", categoryCode.ToString());
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
            return BaseDAO.IsExist(tableName, keyName, code.ToString());
        }


        private static List<Book> LoadProducts(string sql)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cm.ExecuteReader();

            List<Book> products = new List<Book>();
            while (dr.Read())
            {
                Book product = new Book();
                product.Code = (int)dr["Code"];
                product.Title = dr["Title"].ToString();
                product.Description = dr["Description"].ToString();
                product.Pic = BaseDAO.GetValue<byte[]>(dr["Pic"]);
                product.Price = BaseDAO.GetValue<decimal>(dr["Price"]);
                product.Discount = BaseDAO.GetValue<int?>(dr["Discount"], dr["CategoryDiscount"], dr["ParentCategoryDiscount"]);
                product.IsRecommended = BaseDAO.GetValue<bool>(dr["IsRecommended"]);

                products.Add(product);
            }
            conn.Close();
            
            return products;
        }

        public static List<Book> Search(BookSearch productSearch)
        {
            string sql = selectQuery;
            string condition = "";
            if (!string.IsNullOrEmpty(productSearch.Title))
                condition += string.Format("prd.Title Like '%{0}%' And ", productSearch.Title);
            if (productSearch.Author != null)
                condition += string.Format("prd.Author Like '%{0}%' And ", productSearch.Author);
            if (productSearch.OnlyDiscounted)
                condition += "HasDiscount = 1 And ";
            if (productSearch.OnlyRecommended)
                condition += "IsRecommended = 1 And ";

            if (condition != "")
            {
                condition = condition.Substring(0, condition.Length - "And ".Length);
                sql += " Where " + condition;
            }

            return LoadProducts(sql);
        }
    }
}