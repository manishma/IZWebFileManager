using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Session property to define FileBrowser and allwed privilege
/// </summary>
namespace MB.FileBrowser
{
    public class MagicSession
    {

        public Boolean DenyAll
        {
            get
            {
                return !(ReadOnly || Delete || Write);
            }
            set
            {
                if (value)
                    ReadOnly = Delete = Write = false;
                else if (DenyAll)
                    ReadOnly = true;
            }
        }

        private Boolean _ReadOnly;

        public Boolean ReadOnly
        {
            get
            {
                return _ReadOnly || (!(_Delete && _Write));
            }
            set
            {
                _ReadOnly = value;
                if (value)
                {
                    Write = false;
                    Delete = false;
                }
            }
        }

        private Boolean _Write;

        public Boolean Write
        {
            get
            {
                return (_Write || _Delete) && !_ReadOnly;
            }
            set
            {
                _Write = value;
            }
        }

        private Boolean _Delete;

        public Boolean Delete
        {
            get
            {
                return _Delete && _Write && !_ReadOnly;
            }
            set
            {
                _Delete = value;
                if (value) _Write = true;
            }
        }

        public Boolean Upload
        {
            get { return Write; }
            set { Write = value; }
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
                    HttpContext.Current.Session["__MagicSession__"] = session;
                }
                return session;
            }
        }
    }
}