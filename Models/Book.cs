using System.Xml.Serialization;

namespace BookStore.Models
{
    public class Book : IBook
    {
        private byte[] pic = null;

        [XmlAttribute]
        public int Code { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int InStock { get; set; }

        [XmlIgnore]
        public byte[] Pic
        {
            get { return pic; } 
            set { pic = value; }
        }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int? Discount { get; set; }

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