using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStore.Models
{
    public class ProductSearch
    {
        public int? CategoryCode { get; set; }
        public int? ParentCategoryCode { get; set; }
        public string Title { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool OnlyDiscounted { get; set; }
        public bool OnlyRecommended { get; set; }
    }
}
