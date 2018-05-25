
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightStore.DataAccess;
using LightStore.Models;

namespace LightStore.Web.Controllers
{
    public partial class ProductController
    {
        private Product MakeProduct(EditProductViewModel editProductViewModel)
        {
            Product product = new Product();
            product.Code = editProductViewModel.Code;
            product.Title = editProductViewModel.Title;
            product.Description = editProductViewModel.Description;
            product.Category = CategoryDAO.Get(editProductViewModel.CategoryCode);
            product.Length = editProductViewModel.Length;
            product.Width = editProductViewModel.Width;
            product.Height = editProductViewModel.Height.GetValueOrDefault();
            product.Price = editProductViewModel.Price;
            product.Colors = editProductViewModel.Colors;
            product.Discount = editProductViewModel.Discount;
            product.IsRecommended = editProductViewModel.IsRecommended;

            return product;
        }

        private EditProductViewModel MakeEditProductViewModel(Product product)
        {
            EditProductViewModel editProductViewModel = new EditProductViewModel();
            editProductViewModel.Code = product.Code;
            editProductViewModel.Title = product.Title;
            editProductViewModel.Description = product.Description;
            editProductViewModel.CategoryCode = product.Category.Code;
            editProductViewModel.Colors = product.Colors;
            editProductViewModel.Length = product.Length;
            editProductViewModel.Width = product.Width;
            editProductViewModel.Height = product.Height;
            editProductViewModel.Price = product.Price;
            editProductViewModel.Discount = product.Discount;
            editProductViewModel.IsRecommended = product.IsRecommended;

            return editProductViewModel;
        }

        private byte[] MakePicFromRequestFile(string imageId)
        {
            HttpPostedFileBase picFile = Request.Files[imageId];
            if (picFile != null && picFile.ContentLength > 0)
            {
                byte[] pic = new byte[picFile.ContentLength];
                picFile.InputStream.Read(pic, 0, picFile.ContentLength);
                return pic;
            }
            else
                return null;
        }

       
    }
}