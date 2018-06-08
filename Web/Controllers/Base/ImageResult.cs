using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers.Base
{
    public class ImageResult : ActionResult
    {
        public byte[] ImageBytes { get; set; }

        public ImageResult(byte[] sourceStream)
        {
            ImageBytes = sourceStream;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "jpg";

            if (ImageBytes != null)
            {
                MemoryStream stream = new MemoryStream(ImageBytes);
                stream.WriteTo(response.OutputStream);
                stream.Dispose();
            }
        }
    }
}
