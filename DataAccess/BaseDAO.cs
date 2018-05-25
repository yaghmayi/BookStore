using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Web;

namespace LightStore.DataAccess
{
    public static class  BaseDAO
    {
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        }

        public static bool IsExist(string tableName, string keyName, string keyValue)
        {
            SqlConnection conn = GetSqlConnection();
            string sql = string.Format("select count(*) from {0} where {1}='{2}'", tableName, keyName, keyValue);
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            object executeScalar = cm.ExecuteScalar();
            int count = 0;
            if (executeScalar != DBNull.Value)
                count = (int)executeScalar;

            return count > 0;
        }

        public static int GetNextID(string tableName, string keyName)
        {
            SqlConnection conn = GetSqlConnection();
            string sql = string.Format("select max({0}) + 1 from {1}", keyName, tableName);
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            object executeScalar = cm.ExecuteScalar();
            int nextCode = 1;
            if (executeScalar != DBNull.Value)
                nextCode = (int)executeScalar;

            return nextCode;
        }

        public static void Delete(string tableName, string fieldName, string fieldValue)
        {
            SqlConnection conn = GetSqlConnection();
            string sql = string.Format("delete from {0} where {1}='{2}'", tableName, fieldName, fieldValue);
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            cm.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateField(string tableName, string fieldName, string oldValue, string newValue)
        {
            SqlConnection conn = GetSqlConnection();
            string sql = string.Format("Update {0} set {1}={2} where {1}={3}", tableName, fieldName, GetDBValue(newValue), GetDBValue(oldValue));
            SqlCommand cm = new SqlCommand(sql, conn);
            conn.Open();
            cm.ExecuteNonQuery();

            conn.Close();
        }

        public static T GetValue<T>(params object[] fieldValues)
        {
            foreach (object fieldValue in fieldValues)
            {
                T value = GetValue<T>(fieldValue);
                if (value != null)
                    return value;
            }
            return default(T);
        }

        public static T GetValue<T>(object fieldValue)
        {
            Type type = typeof(T);
            if (fieldValue != DBNull.Value)
            {
                Type target = typeof(T);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    target = type.GetGenericArguments().First();

                return (T)Convert.ChangeType(fieldValue, target);
            }
            else
                return default(T);
        }

        public static string GetDBValue(object fieldValue)
        {
            if (fieldValue is bool)
            {
                if ((bool)fieldValue)
                    return "1";
                else
                    return "0";
            }
            else if (fieldValue == null)
                return "Null";
            else
                return string.Format("'{0}'", fieldValue);
        }
    }
}