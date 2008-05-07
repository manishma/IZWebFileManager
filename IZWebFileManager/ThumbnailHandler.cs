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
		public bool IsReusable {
			get { return true; }
		}

		public void ProcessRequest (HttpContext context) {
			context.Response.ContentType = "image/jpeg";
			int size = 92;

			string vPath = HttpUtility.UrlDecode (context.Request.Url.Query.Substring (1));
			string path = context.Request.MapPath (vPath);
			Image img = Image.FromFile (path);
			if (img.Width > size || img.Height > size) {
				int tw = img.Width > img.Height ? size : (img.Width * size) / img.Height;
				int th = img.Width > img.Height ? (img.Height * size) / img.Width : size;
				img = img.GetThumbnailImage (tw, th, null, IntPtr.Zero);
			}
			img.Save (context.Response.OutputStream, ImageFormat.Jpeg);
		}
	}
}
