using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Session property to define FileBrowser and allwed privilege
/// </summary>
namespace MB.FileBrowser
{
    public enum FileBrowserAccess
    {
        DenyAll, ReadOnly, Write, Delete
    }
    public class MagicSession
    {
        private FileBrowserAccess _access = FileBrowserAccess.DenyAll;

        public Boolean DenyAll
        {
            get
            {
                return _access == FileBrowserAccess.DenyAll;
            }
            set
            {
                _access = FileBrowserAccess.DenyAll;
            }
        }

        public Boolean ReadOnly
        {
            get
            {
                return _access == FileBrowserAccess.ReadOnly;
            }
            set
            {
                _access = FileBrowserAccess.ReadOnly;
            }
        }

        public Boolean Write
        {
            get
            {
                return (_access == FileBrowserAccess.Write) || (_access == FileBrowserAccess.Delete) ;
            }
            set
            {
                _access = FileBrowserAccess.Write;
            }
        }


        public Boolean Delete
        {
            get
            {
                return _access == FileBrowserAccess.Delete;
            }
            set
            {
               _access = FileBrowserAccess.Delete;
            }
        }

        public Boolean Upload
        {
            get { return Write; }
            //set { Write = value; }
        }


        private MagicSession()
        {
            DenyAll = true;
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