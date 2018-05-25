using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightStore.Models
{
    public class CategoryViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; }
        public int ProductsNumber { get; set; }

        public CategoryViewModel()
        {
            SubCategories = new List<CategoryViewModel>();
        }
    }
}