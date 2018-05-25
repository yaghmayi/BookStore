using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LightStore.Models
{
    public enum Color
    {
        White = 1, 
        Black = 2,
        Blue = 3,
        Red = 4,
        Brown = 5
    }

    public class Product
    {
        private byte[] pic = null;

        public int Code { get; set; }
        public string Title { get; set; }
        public byte[] Pic
        {
            get { return pic; } 
            set { pic = value; }
        }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Color> Colors { get; set; }

        [Required]
        public int? Length { get; set; }

        [Required]
        public int? Width { get; set; }

        public int? Height { get; set; }

        public Category Category { get; set; }

        public int? Discount { get; set; }
        public bool IsRecommended { get; set; }

        public decimal SalePrice
        {
            get
            {
                return Price - (Discount.GetValueOrDefault() * Price / 100);
            }
        }

        public bool HasDiscount
        {
            get
            {
               return Discount != null && Discount > 0;
            }
        }
    }
}