using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStore.Models
{
    public class BookSearch
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool OnlyDiscounted { get; set; }
        public bool OnlyRecommended { get; set; }
    }
}
