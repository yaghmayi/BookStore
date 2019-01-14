using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BookStore.DataAccess
{
    public class DAOHelper
    {
        public static String dataFolder
        {
            get
            {
                String dataSourceFolder = ConfigurationManager.AppSettings.Get("DataSourceFolder");

                return AppDomain.CurrentDomain.BaseDirectory + "\\" + dataSourceFolder;
            }
        }
    }
}
