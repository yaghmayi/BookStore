using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LightStore.DataAccess;
using LightStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightStore.UnitTest.DataAccess
{
    [TestClass]
    public class CategoryDAOTest
    {
        [TestInitialize]
        [TestCleanup]
        public void CleanUp()
        {
            TestUtil.ClearTable("Categories");

            Category category = new Category() { Code = 201, Name = "Electronic" };
            CategoryDAO.Save(category);

            Category subCategory = new Category() { Code = 202, Name = "LabTop", ParentCode = 201 };
            CategoryDAO.Save(subCategory);
        }

        [TestMethod]
        public void Save()
        {
            List<Category> categories = CategoryDAO.GetAll();
            Assert.AreEqual(1, categories.Count);
            Assert.IsNull(categories.First().ParentCode);
            Assert.AreEqual(1, categories.First().SubCategories.Count);

            Category category = CategoryDAO.Get(201);
            Assert.IsNull(category.ParentCode);
            Assert.AreEqual(1, category.SubCategories.Count);

            Category subCategory = CategoryDAO.Get(202);
            Assert.AreEqual(201, subCategory.ParentCode);
        }

        [TestMethod]
        public void GetAllCategoryViews()
        {
            Category subCategory = CategoryDAO.Get(202);
            Product product = new Product() { Code = 101, Title = "P1", Category = subCategory};
            ProductDAO.Save(product);

            List<CategoryViewModel> categoriesViews = CategoryDAO.GetAllCategoryViews();
            Assert.AreEqual(1, categoriesViews.Count);
            Assert.AreEqual("Electronic", categoriesViews.First().Name);
            Assert.AreEqual(1, categoriesViews.First().SubCategories.Count);
            Assert.AreEqual("LabTop", categoriesViews.First().SubCategories.First().Name);
            Assert.AreEqual(1, categoriesViews.First().SubCategories.First().ProductsNumber);
        }

        [TestMethod]
        public void Discount()
        {
            Category category = new Category() { Code = 301, Name = "House Furniture", Discount = 10};
            CategoryDAO.Save(category);

            Category subCategory = new Category() { Code = 302, Name = "Kitchen", ParentCode = 301 };
            CategoryDAO.Save(subCategory);

            category = CategoryDAO.Get(301);
            Assert.AreEqual(10, category.Discount);

            subCategory = CategoryDAO.Get(302);
            Assert.AreEqual(10, subCategory.Discount);

            subCategory.Discount = 30;
            CategoryDAO.Save(subCategory);
            subCategory= CategoryDAO.Get(302);
            Assert.AreEqual(30, subCategory.Discount);
        }

    }
}
