using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace IZ.WebFileManager
{
    public class ThumbnailHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            int size = 92;

            string vPath = HttpUtility.UrlDecode(context.Request.Url.Query.Substring(1));
            string path = context.Request.MapPath(vPath);

            using (var original = Image.FromFile(path))
            {
                if (original.Width > size || original.Height > size)
                {
                    int tw = original.Width > original.Height ? size : (original.Width * size) / original.Height;
                    int th = original.Width > original.Height ? (original.Height * size) / original.Width : size;

                    using (var thumb = original.GetThumbnailImage(tw, th, null, IntPtr.Zero))
                    {
                        thumb.Save(context.Response.OutputStream, ImageFormat.Jpeg);

                    }
                }
                else
                {
                    original.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
            }

        }
    }
}
