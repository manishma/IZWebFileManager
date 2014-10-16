using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IZ.WebFileManager;

/// <summary>
/// Session property to define FileBrowser and allwed privilege
/// </summary>
namespace MB.FileBrowser
{
    public class MagicSession
    {
        public AccessMode FileBrowserAccessMode = AccessMode.DenyAll;

        public Boolean DenyAll
        {
            get
            {
                return FileBrowserAccessMode == AccessMode.DenyAll;
            }
            set
            {
                FileBrowserAccessMode = AccessMode.DenyAll;
            }
        }

        public Boolean ReadOnly
        {
            get
            {
                return FileBrowserAccessMode == AccessMode.ReadOnly;
            }
            set
            {
                FileBrowserAccessMode = AccessMode.ReadOnly;
            }
        }

        public Boolean Write
        {
            get
            {
                return (FileBrowserAccessMode == AccessMode.Write) || (FileBrowserAccessMode == AccessMode.Delete) ;
            }
            set
            {
                FileBrowserAccessMode = AccessMode.Write;
            }
        }


        public Boolean Delete
        {
            get
            {
                return FileBrowserAccessMode == AccessMode.Delete;
            }
            set
            {
               FileBrowserAccessMode = AccessMode.Delete;
            }
        }

        private Boolean _default;

        public Boolean Default
        {
            get { return FileBrowserAccessMode == AccessMode.Default; }
            set { FileBrowserAccessMode = AccessMode.Default; }
        }
        

        public Boolean Upload
        {
            get { return Write; }
            //set { Write = value; }
        }


        private MagicSession()
        {
            FileBrowserAccessMode = AccessMode.Default;
        }

        public static MagicSession Current
        {
            get
            {
                MagicSession session = (MagicSession)HttpContext.Current.Session["__FB_MagicSession__"];
                if (session == null)
                {
                    session = new MagicSession();
                    HttpContext.Current.Session["__FB_MagicSession__"] = session;
                }
                return session;
            }
        }
    }
}