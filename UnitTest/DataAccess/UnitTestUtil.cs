using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LightStore.DataAccess;


namespace LightStore.UnitTest.DataAccess
{
    public static class TestUtil
    {
        public static void ClearTable(string tableName)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(string.Format("Delete from {0}", tableName), conn);
            conn.Open();
            cm.ExecuteNonQuery();
            conn.Close();
        }

        public static int GetRecordsCount(string tableName)
        {
            SqlConnection conn = BaseDAO.GetSqlConnection();
            SqlCommand cm = new SqlCommand(string.Format("select count(*) from {0}", tableName), conn);
            conn.Open();
            int count = (int) cm.ExecuteScalar();
            conn.Close();

            return count;
        }
    }
}
