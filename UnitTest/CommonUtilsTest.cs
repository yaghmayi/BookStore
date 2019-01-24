using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.UnitTest
{
    [TestClass]
    public class CommonUtilsTest
    {
        [TestMethod]
        public void GetPageName()
        {
            Assert.AreEqual("login", CommonUtils.GetPageName("http://localhost:13985/Customer/Login"));
        }
    }
}
