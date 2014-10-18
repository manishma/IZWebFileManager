<%@ WebHandler Language="C#" Class="MB.FileBrowser.MyUpload" %>

using System;
using System.Web;
using System.Linq;
using System.IO;
using IZ.WebFileManager;

namespace MB.FileBrowser
{
    public class MyUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        const int MaxAllowedSize = 50; //Megabyte
        public void ProcessRequest(HttpContext context)
        {
            string filename, imagepath, cultureName;
            AjaxJsonResponse response = new AjaxJsonResponse();
            try
            {
                filename = context.Request["qqfilename"];
                imagepath = context.Request["dest"];
                cultureName = context.Request["culture"];
                System.Globalization.CultureInfo culture = (String.IsNullOrEmpty(cultureName) ?
                    System.Globalization.CultureInfo.CurrentCulture :
                    new System.Globalization.CultureInfo(cultureName));
                string successMsg = GetResource("Upload_success", culture, "File \"{0}\" was successfully handled.");
                response.exitcode = 0;
                response.data = null;
                response.success = true;
                response.msg = String.Format(successMsg, filename);
                string extension = System.IO.Path.GetExtension(filename).Remove(0, 1).ToLower();
                if (context.Request.Files[0].InputStream.Length > 1024 * 1024 * MaxAllowedSize)
                {
                    string msg = GetResource("Upload_Error_1", culture, "File exceeds maximum size allowed ({0} Mb)");
                    response.success = false;
                    response.exitcode = 1;
                    response.msg = String.Format(msg, MaxAllowedSize);
                }
                else if (!AllowedFilesType.GetAllowed().Contains(extension))
                {
                    response.success = false;
                    response.exitcode = 2;
                    response.msg = GetResource("Upload_Error_2", culture, "File type not allowed or not supported.");
                }
                else if (!MagicSession.Current.Upload)
                {
                    response.success = false;
                    response.exitcode = 3;
                    response.msg = GetResource("Upload_Error_3", culture, "User does not have sufficient privileges.");
                }
                else
                {
                    for (int k = 0; k < context.Request.Files.Count; k++)
                    {
                        string destPath = context.Server.MapPath(VirtualPathUtility.AppendTrailingSlash(imagepath) + filename);
                        int i = 1;
                        while (File.Exists((destPath)))
                        {
                            destPath = Path.GetFileNameWithoutExtension(destPath) + "(" + i.ToString() + ")" + Path.GetExtension(destPath);
                            i++;
                        }
                        context.Request.Files[0].SaveAs(destPath);
                    }
                }
            }
            catch (Exception e)
            {

                response.success = false;
                response.exitcode = 4;
                response.msg = e.Message;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = serializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.Charset = "UTF-8";
            context.Response.Write(json);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GetResource(string key, System.Globalization.CultureInfo culture, string defaultValue)
        {

            if (HttpContext.GetGlobalResourceObject("IZWebFileManagerResource", key, culture) != null)
                return HttpContext.GetGlobalResourceObject("IZWebFileManagerResource", key, culture).ToString();

            if (HttpContext.GetGlobalResourceObject("IZWebFileManagerResource", key, System.Globalization.CultureInfo.InvariantCulture) != null)
                return HttpContext.GetGlobalResourceObject("IZWebFileManagerResource", key, System.Globalization.CultureInfo.InvariantCulture).ToString();

            return defaultValue;
        }

    }
}