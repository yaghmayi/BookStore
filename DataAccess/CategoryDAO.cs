using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LightStore.Models;

namespace LightStore.DataAccess
{
    public static class CategoryDAO
    {
        private static string tableName = "Categories";
        private static string keyName = "Code";

        private static string selectQuery
        {
            get
            {
                return "select *, (select Discount from Categories p where p.Code = c.ParentCode) as ParentDiscount from Categories c ";
            }
        }

        private static string selectWithProductsNumbersQuery
        {
            get
            {
                return "select *, (select Discount from Categories p where p.Code = c.ParentCode) as ParentDiscount, " +
                        "(select count(*) from Products as prd where prd.CategoryCode = c.Code) as productsNumber " +
                        "from Categories c ";
            }
        }

        public static List<Category> GetAll()
        {
            string sql = selectQuery + "Where ParentCode is null";
            List<Category> categories = LoadCategories(sql);

            foreach (Category category in categories)
            {
                sql = string.Format(selectQuery + "Where ParentCode = {0}", category.Code);
                category.SubCategories = LoadCategories(sql);
            }

            return categories;
        }

        public static List<CategoryViewModel> GetAllCategoryViews()
        {
            string sql = selectWithProductsNumbersQuery + "Where ParentCode is null";
            List<CategoryViewModel> categoryViews = LoadCategories(sql).ConvertAll(category => new CategoryViewModel() {Code = category.Code, Name = category.Name});
            foreach (CategoryViewModel categoryView in categoryViews)
            {
                sql = selectWithProductsNumbersQuery + string.Format("Where ParentCode={0}", categoryView.Code);
                categoryView.SubCategories = LoadCategoryViews(sql);
            }
            
            return categoryViews;
        } 

        public static Category Get(int code)
        {
            string sql =  selectQuery + string.Format("Where Code='{0}'", code);
            Category category = LoadCategories(sql).FirstOrDefault();

            if (category != null)
            {
                sql = string.Format(selectQuery + "Where ParentCode = {0}", category.Code);
                category.SubCategories = LoadCategories(sql);
            }

            return category;
        }

        public static void Save(Category category)
        {
            string sql = "";
            if (IsExistCategory(category.Code))
            {
                sql = string.Format("Update Categories set Name='{0}' , ParentCode={1}, Discount={2} Where Code='{3}'",
                                     category.Name, BaseDAO.GetDBValue(category.ParentCode), BaseDAO.GetDBValue(category.Discount), category.Code);
            }
            else
            {
                sql = string.Format("Insert into Categories (Code, Name, Discount, ParentCode) Values ('{0}','{1}',{2}, {3})",
                                     category.Code, category.Name, BaseDAO.GetDBValue(category.Discount), BaseDAO.GetDBValue(category.ParentCode));
            }

            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            cm.ExecuteNonQuery();
            conn.Close();
        }

        public static int GetNextCode()
        {
            return BaseDAO.GetNextID(tableName, keyName);
        }

        public static void Delete(int code)
        {
            BaseDAO.Delete(tableName, keyName, code.ToString());
        }

        private static bool IsExistCategory(int code)
        {
            return BaseDAO.IsExist(tableName, keyName, code.ToString());
        }

        private static List<Category> LoadCategories(string sql)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);

            conn.Open();
            SqlDataReader dr = cm.ExecuteReader();
            List<Category> categories = new List<Category>();

            while (dr.Read())
            {
                Category category = new Category();
                category.Code = (int)dr["Code"];
                category.Name = dr["Name"].ToString();
                category.ParentCode = BaseDAO.GetValue<int?>(dr["ParentCode"]);
                category.Discount = BaseDAO.GetValue<int?>(dr["Discount"], dr["ParentDiscount"]);

                categories.Add(category);
            }

            return categories;
        }

        private static List<CategoryViewModel> LoadCategoryViews(string sql)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(sql, conn);

            conn.Open();
            SqlDataReader dr = cm.ExecuteReader();
            List<CategoryViewModel> categoryViews = new List<CategoryViewModel>();

            while (dr.Read())
            {
                CategoryViewModel categoryView = new CategoryViewModel();
                categoryView.Code = (int)dr["Code"];
                categoryView.Name = dr["Name"].ToString();
                categoryView.ProductsNumber = BaseDAO.GetValue<int>(dr["ProductsNumber"]);

                categoryViews.Add(categoryView);
            }
            conn.Close();

            return categoryViews;
        }

        


    }
}