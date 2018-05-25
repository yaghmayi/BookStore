using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LightStore.Models
{
    public class Category
    {
        [Required]
        public int Code { get; set; }

        [Required]
        public string Name { get; set; }

        public int? ParentCode { get; set; }
        
        public List<Category> SubCategories { get; set; }

        public int? Discount { get; set; }

        public Category()
        {
            SubCategories = new List<Category>();
        }
    }
}