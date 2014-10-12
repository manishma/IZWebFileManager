using System;
using System.IO;
using IZ.WebFileManager;
using System.Security.AccessControl;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;

namespace MB.FileBrowser
{
    public partial class FileBrowser : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        protected AjaxJsonResponse ajaxResponse = new AjaxJsonResponse();
        public string Opener;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.Host.IndexOf("localhost") > -1)
                MagicSession.Current.Write = true;


            CultureInfo culture;
            try
            {
                culture = new CultureInfo(Request["langCode"]);
            }
            catch (Exception)
            {
                culture = CultureInfo.CurrentCulture;
            }


            String cbReference =
                Page.ClientScript.GetCallbackEventReference(this,
                "arg", "ReceiveServerData", "context");
            String callbackScript;
            callbackScript = "function CallServer(arg, context)" +
                "{ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                "CallServer", callbackScript, true);

            if (!IsPostBack)
            {
                /**
                 * -------- Parameters --------------
                 * CKEDITOR automatically call FileManager service adding two custom parameters 
                 * CKEditorFuncNum e type.
                 * First parameter allows you to pass choosen file url back to CKEDITOR 
                 * through callback function
                 * Type paramete is used to restrict the file search to a 
                 * specific folder
                 * 
                 * Tiny MCE 4 use the type parameter and field parameter wich is 
                 * the id of the field whose value must be set.
                 * 
                 * 
                 * Other recognized parameters
                 * caller: 
                 *      allowed values: ckeditor, tinymce, parent, top
                 *      default: caller id defaulted to ckeditor if the CKEditor parameter is specified otherwise to parent
                 *      Idicates the object to wich the callback must be return  
                 *      
                 * fn:
                 *      allowed values: any string
                 *      default: null
                 *      Function name to be called.
                 * 
                 * langCode:
                 *      allowed value: a standard language code
                 *      default: current language
                 *      CKEdito pass this paramenter automatically
                 *      
                 */

                int fnumber = 0;
                string caller, fn;

                // the caller is CKEditor
                if (!string.IsNullOrEmpty(Request["CKEditor"]))
                {
                    caller = "ckeditor";
                }
                else
                    caller = (String.IsNullOrEmpty(Request["caller"]) ? "parent" : Request["caller"]);

                HF_Opener.Value = caller;

                fn = Request["fn"];

                if (!String.IsNullOrEmpty(fn))
                    HF_CallBack.Value = fn;

                if (int.TryParse(Request["CKEditorFuncNum"], out fnumber))
                    HF_CKEditorFunctionNumber.Value = fnumber.ToString();

                if (!String.IsNullOrEmpty(Request["field"]))
                    HF_Field.Value = Request["field"];

                string type = "";
                string mainRoot = "~/userfiles";

                if (FileManager1.Culture == null)
                    FileManager1.Culture = culture;

                HF_CurrentCulture.Value = FileManager1.Culture.Name;

                FileManager1.CustomToolbarButtons[0].Text = FileManager1.Controller.GetResourceString("View_file", "View File");
                Upload_button.InnerText = FileManager1.Controller.GetResourceString("Upload_file_click", "Click here to download a file");
                DND_message.InnerText = FileManager1.Controller.GetResourceString("Upload_dnd", "Or drag 'nd drop one or more files on the above area");

                if (!String.IsNullOrEmpty(FileManager1.MainDirectory))
                    mainRoot = FileManager1.MainDirectory;
                //mainRoot = ResolveClientUrl(mainRoot);
                if (!Directory.Exists(Server.MapPath(ResolveClientUrl(mainRoot))))
                    throw new Exception("User directory with write privileges is needed.");

                DirectoryInfo mainRootInfo = new DirectoryInfo(Server.MapPath(ResolveClientUrl(mainRoot)));

                mainRootInfo.CreateSubdirectory("images");
                mainRootInfo.CreateSubdirectory("files");
                mainRootInfo.CreateSubdirectory("flash");
                mainRootInfo.CreateSubdirectory("media");

                if (!String.IsNullOrEmpty(Request["type"]))
                {
                    type = Request["type"];
                }

                RootDirectory images, flash, files, media;
                // Display text of root folders are localized using WebFileBrowser resources files
                // in "/App_GlobalResources/WebFileManager" and GetResoueceString method
                // of FileManager.Controller class
                switch (type)
                {
                    case "images":
                    case "image":
                        FileManager1.RootDirectories.Clear();
                        images = new RootDirectory();
                        images.ShowRootIndex = false;
                        images.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "images";
                        images.Text = FileManager1.Controller.GetResourceString("Root_Image", "Images");
                        images.LargeImageUrl = "~/FileBrowser/img/32/camera.png";
                        images.SmallImageUrl = "~/FileBrowser/img/16/camera.png";
                        FileManager1.RootDirectories.Add(images);
                        break;
                    case "flash":
                        FileManager1.RootDirectories.Clear();
                        flash = new RootDirectory();
                        flash.ShowRootIndex = false;
                        flash.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "flash";
                        flash.LargeImageUrl = "~/FileBrowser/img/32/folder-flash.png";
                        flash.SmallImageUrl = "~/FileBrowser/img/16/folder-flash.png";
                        flash.Text = FileManager1.Controller.GetResourceString("Root_Flash", "Flash Movies");
                        FileManager1.RootDirectories.Add(flash);
                        break;
                    case "files":
                        FileManager1.RootDirectories.Clear();
                        files = new RootDirectory();
                        files.ShowRootIndex = false;
                        files.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "files";
                        files.LargeImageUrl = "~/FileBrowser/img/32/folder-document-alt.png";
                        files.SmallImageUrl = "~/FileBrowser/img/16/folder-document-alt.png";
                        files.Text = FileManager1.Controller.GetResourceString("Root_File", "Files");
                        FileManager1.RootDirectories.Add(files);
                        break;
                    case "media":
                        FileManager1.RootDirectories.Clear();
                        media = new RootDirectory();
                        media.ShowRootIndex = false;
                        media.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "media";
                        media.LargeImageUrl = "~/FileBrowser/img/32/folder-video-alt.png";
                        media.SmallImageUrl = "~/FileBrowser/img/16/folder-video-alt.png";
                        media.Text = FileManager1.Controller.GetResourceString("Root_Media", "Media");
                        FileManager1.RootDirectories.Add(media);
                        break;
                    default:
                        FileManager1.RootDirectories.Clear();
                        files = new RootDirectory();
                        files.ShowRootIndex = false;
                        files.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "files";
                        files.LargeImageUrl = "~/FileBrowser/img/32/folder-document-alt.png";
                        files.SmallImageUrl = "~/FileBrowser/img/16/folder-document-alt.png";
                        // Display text of root folders are localized using WebFileBrowser resources files
                        // in "/App_GlobalResources/WebFileManager" and GetResoueceString method
                        // of FileManager.Controller class
                        files.Text = FileManager1.Controller.GetResourceString("Root_File", "Files");
                        FileManager1.RootDirectories.Add(files);

                        flash = new RootDirectory();
                        flash.ShowRootIndex = false;
                        flash.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "flash";
                        flash.LargeImageUrl = "~/FileBrowser/img/32/folder-flash.png";
                        flash.SmallImageUrl = "~/FileBrowser/img/16/folder-flash.png";
                        flash.Text = FileManager1.Controller.GetResourceString("Root_Flash", "Flash Movies");
                        FileManager1.RootDirectories.Add(flash);

                        images = new RootDirectory();
                        images.ShowRootIndex = false;
                        images.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "images";
                        images.Text = FileManager1.Controller.GetResourceString("Root_Image", "Images");
                        images.LargeImageUrl = "~/FileBrowser/img/32/camera.png";
                        images.SmallImageUrl = "~/FileBrowser/img/16/camera.png";
                        FileManager1.RootDirectories.Add(images);

                        media = new RootDirectory();
                        media.ShowRootIndex = false;
                        media.DirectoryPath = VirtualPathUtility.AppendTrailingSlash(mainRoot) + "media";
                        media.LargeImageUrl = "~/FileBrowser/img/32/folder-video-alt.png";
                        media.SmallImageUrl = "~/FileBrowser/img/16/folder-video-alt.png";
                        media.Text = FileManager1.Controller.GetResourceString("Root_Media", "Media");
                        FileManager1.RootDirectories.Add(media);

                        break;
                }
            }

            if (MagicSession.Current.DenyAll)
            {
                FileManager1.Visible = false;
                Panel_upload.Visible = false;
                Panel_deny.Visible = true;
                Literal content = new Literal();
                content.Text = "<h1>" + FileManager1.Controller.GetResourceString("Upload_Error_3", "User does not have sufficient privileges.") + "<br/>&nbsp;</h1>";
                Panel_deny.Controls.Add(content);
            }
            else if (MagicSession.Current.ReadOnly)
            {
                FileManager1.Visible = true;
                Panel_upload.Visible = true;
                Panel_deny.Visible = false;
                Literal content = new Literal();
                FileManager1.ReadOnly = true;
            }
            else if (MagicSession.Current.Write && !MagicSession.Current.Delete)
            {
                FileManager1.Visible = true;
                Panel_upload.Visible = true;
                Panel_deny.Visible = false;
                FileManager1.ReadOnly = false;
                FileManager1.AllowDelete = false;
                FileManager1.AllowOverwrite = false;
            }
            else if (MagicSession.Current.Delete)
            {
                FileManager1.Visible = true;
                Panel_upload.Visible = true;
                Panel_deny.Visible = false;
                FileManager1.ReadOnly = false;
                FileManager1.AllowDelete = true;
                FileManager1.AllowOverwrite = true;
            }

        }
        public void RaiseCallbackEvent(String eventArgument)
        {
            string[] cmds = eventArgument.Split(new char[] { ',' });
            ajaxResponse.command = cmds[0].ToLower();
            switch (cmds[0].ToLower())
            {
                case "showfile":
                    if (FileManager1.CurrentDirectory != null)
                        ajaxResponse.data = VirtualPathUtility.AppendTrailingSlash(FileManager1.CurrentDirectory.VirtualPath) + cmds[1];
                    break;
                case "upload":
                    if (FileManager1.CurrentDirectory != null)
                        ajaxResponse.data = VirtualPathUtility.AppendTrailingSlash(FileManager1.CurrentDirectory.VirtualPath);
                    break;
                default:
                    break;
            }

        }
        public String GetCallbackResult()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(ajaxResponse);

        }

    }
}