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
using Color = LightStore.Models.Color;

namespace LightStore.DataAccess
{
    public static class ProductDAO
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

        public static void Save(Product product)
        {
            string sql = "";
            if (IsExist(product.Code))
            {
                sql = string.Format("Update Products set CategoryCode='{0}', Title='{1}', Description='{2}', Colors={3}, " +
                                    "Length={4}, Width={5}, Height={6}, Price={7}, Discount={8}, IsRecommended={9}, Pic=@Pic Where Code='{10}'",
                                     product.Category.Code, product.Title, product.Description, GetColorsValue(product.Colors),
                                     BaseDAO.GetDBValue(product.Length), BaseDAO.GetDBValue(product.Width), BaseDAO.GetDBValue(product.Height), 
                                     product.Price, BaseDAO.GetDBValue(product.Discount), BaseDAO.GetDBValue(product.IsRecommended), product.Code);
            }
            else
            {
                sql = string.Format("Insert into Products (Code, CategoryCode, Title, Description, Colors, Length, Width, Height, Price, Discount, IsRecommended, Pic) " +
                                    "Values ('{0}','{1}', '{2}', '{3}',{4}, {5}, {6}, {7}, {8}, {9}, {10}, @Pic)",
                                    product.Code, product.Category.Code, product.Title, product.Description, GetColorsValue(product.Colors),
                                    BaseDAO.GetDBValue(product.Length), BaseDAO.GetDBValue(product.Width), BaseDAO.GetDBValue(product.Height), 
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

        public static List<Product> GetByCategoryCode(int categoryCode)
        {
            string sql = selectQuery + string.Format(" Where prd.CategoryCode='{0}'", categoryCode);
            return LoadProducts(sql);
        }

        public static Product Get(int code)
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
            Product product = Get(productCode);
            if (product != null)
                return product.Pic;
            else
                return null;
        }

        private static bool IsExist(int code)
        {
            return BaseDAO.IsExist(tableName, keyName, code.ToString());
        }

        private static List<Color> GetColors(string colorCodes)
        {
            List<Color> colors = new List<Color>();
            if (!string.IsNullOrEmpty(colorCodes))
            {
                foreach (string colorCode in colorCodes.Split(','))
                {
                    Color color = (Color)Enum.Parse(typeof(Color), colorCode.Trim());
                    colors.Add(color);
                }
            }

            return colors;
        }

        private static string GetColorsValue(List<Color> colors)
        {
            string val = "";
            if (colors != null && colors.Count > 0)
            {
                foreach (Color color in colors)
                {
                    val += (int) color + ",";
                }

                val = string.Format("'{0}'", val.Trim(','));
            }
            else
                val = "NULL";

            return val;
        }

        private static List<Product> LoadProducts(string sql)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cm.ExecuteReader();

            List<Product> products = new List<Product>();
            while (dr.Read())
            {
                Product product = new Product();
                product.Code = (int)dr["Code"];
                product.Title = dr["Title"].ToString();
                product.Description = dr["Description"].ToString();
                product.Category = CategoryDAO.Get((int)dr["CategoryCode"]);
                product.Colors = GetColors(BaseDAO.GetValue<string>(dr["Colors"]));
                product.Length = BaseDAO.GetValue<int>(dr["Length"]);
                product.Width = BaseDAO.GetValue<int>(dr["Width"]);
                product.Height = BaseDAO.GetValue<int>(dr["Height"]);
                product.Pic = BaseDAO.GetValue<byte[]>(dr["Pic"]);
                product.Price = BaseDAO.GetValue<decimal>(dr["Price"]);
                product.Discount = BaseDAO.GetValue<int?>(dr["Discount"], dr["CategoryDiscount"], dr["ParentCategoryDiscount"]);
                product.IsRecommended = BaseDAO.GetValue<bool>(dr["IsRecommended"]);

                products.Add(product);
            }
            conn.Close();
            
            return products;
        }

        public static List<Product> Search(ProductSearch productSearch)
        {
            string sql = selectQuery;
            string condition = "";
            if (productSearch.CategoryCode != null)
                condition += string.Format("prd.CategoryCode = {0} And ", productSearch.CategoryCode);
            if (!string.IsNullOrEmpty(productSearch.Title))
                condition += string.Format("prd.Title Like '%{0}%' And ", productSearch.Title);
            if (productSearch.MinPrice != null)
                condition += string.Format("prd.SalePrice >= {0} And ", productSearch.MinPrice);
            if (productSearch.MaxPrice != null)
                condition += string.Format("prd.SalePrice <= {0} And ", productSearch.MaxPrice);
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