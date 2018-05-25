using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using LightStore.DataAccess;
using LightStore.Models;

namespace LightStore.Web.Controllers
{
    public partial class ProductController
    {
        public ActionResult ListCategories()
        {
            List<CategoryViewModel> categoryViews = CategoryDAO.GetAllCategoryViews();
            return View(categoryViews);
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult CreateCategory(int? parentCategoryCode)
        {
            Category category = new Category();
            category.Code = CategoryDAO.GetNextCode();
            if (parentCategoryCode != null)
            {
                category.ParentCode = parentCategoryCode.Value;
                ViewData[DataKeys.ParentCategory] = CategoryDAO.Get(parentCategoryCode.Value);
            }

            return View(category);
        }

        public string GetSubCategories(int id)
        {
            List<Category> subCategories = CategoryDAO.Get(id).SubCategories;
            return new JavaScriptSerializer().Serialize(subCategories);
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            CategoryDAO.Save(category);
            if (category.ParentCode != null)
                return RedirectToAction("ListCategories", "Product", new { parentCategoryCode = category.ParentCode });
            else
                return RedirectToAction("ListCategories", "Product");
        }

        public ActionResult ShowCategory(int id)
        {
            Category category = CategoryDAO.Get(id);
            ViewData["Products"] = ProductDAO.GetByCategoryCode(id);

            return View(category);
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult EditCategory(int id)
        {
            Category category = CategoryDAO.Get(id);
            if (category.ParentCode != null)
                ViewData[DataKeys.ParentCategory] = CategoryDAO.Get(category.ParentCode.Value);

            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            CategoryDAO.Save(category);
            return RedirectToAction("ListCategories", "Product");
        }

        [HttpGet]
        [MyAthorize("Admin")]
        public ActionResult DeleteCategory(int id)
        {
            Category category = CategoryDAO.Get(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(Category category)
        {
            ProductDAO.DeleteByCategoryCode(category.Code);
            CategoryDAO.Delete(category.Code);

            return RedirectToAction("ListCategories", "Product");
        }
    }
}