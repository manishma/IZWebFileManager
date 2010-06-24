// Copyright (C) 2006 Igor Zelmanovich <izwebfilemanager@gmail.com>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.Adapters;
using System.Web.UI;

namespace IZ.WebFileManager
{
    class ContextMenu : BaseMenu
    {
        private readonly ContextMenuAdapter contextMenuAdapter;

        public ContextMenu()
        {
            contextMenuAdapter = new ContextMenuAdapter(this);
        }

        protected override ControlAdapter ResolveAdapter()
        {
            return contextMenuAdapter;
        }

        class ContextMenuAdapter : BaseMenuAdapter
        {
            public ContextMenuAdapter(ContextMenu contextMenu)
                : base(contextMenu)
            {
            }

            protected override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "100");
                //writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
                writer.AddAttribute (HtmlTextWriterAttribute.Id, Control.ClientID);
                writer.RenderBeginTag (HtmlTextWriterTag.Div);
            }

            protected override void RenderEndTag(HtmlTextWriter writer)
            {
                writer.RenderEndTag ();
            }

            protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
            {
                RenderDropDownMenu(writer, Control.Items[0].ChildItems, Control.ClientID + "_s");
            }
        }
    }
}
