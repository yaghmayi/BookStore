using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightStore.Models
{
    public class Customer
    {
        private byte[] pic = null;

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] Pic
        {
            get { return pic; }
            set { pic = value; }
        }
    }
}