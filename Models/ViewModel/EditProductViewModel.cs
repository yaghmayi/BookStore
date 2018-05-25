using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightStore.Models
{
    public class EditProductViewModel
    {
        [Required]
        public int Code { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public int? Length { get; set; }

        [Required]
        public int? Width { get; set; }

        public int? Height { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int? Discount { get; set; }

//        [HiddenInput]
//        public byte[] Pic { get; set; }

        public string Description { get; set; }

        public List<Color> Colors { get; set; } 

        [HiddenInput]
        public int CategoryCode { get; set; }

        [HiddenInput]
        public string CategoryName { get; set; }

        public bool IsRecommended { get; set; }

    }
}