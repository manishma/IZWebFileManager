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
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace IZ.WebFileManager
{
    internal class ToolbarMenu : BaseMenu
    {
        private readonly ToolbarMenuAdapter toolbarMenuAdapter;

        public ToolbarMenu()
        {
            toolbarMenuAdapter = new ToolbarMenuAdapter(this);
        }

        protected override ControlAdapter ResolveAdapter()
        {
            return toolbarMenuAdapter;
        }

        internal class ToolbarMenuAdapter : BaseMenuAdapter
        {
            public ToolbarMenuAdapter(ToolbarMenu toolbarMenu)
                : base(toolbarMenu)
            {
            }

            protected override void RenderBeginTag(HtmlTextWriter writer)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }
            protected override void RenderEndTag(HtmlTextWriter writer)
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            protected override void RenderItem(HtmlTextWriter writer, MenuItem item, int position)
            {
                var hasChildren = item.ChildItems.Count > 0;
                var itemControl = GetMenuItemTemplateContainer(item);
                var submenuClientId = itemControl.ClientID + "_s";

                if(hasChildren)
                {
                    writer.AddAttribute("onmouseover", "IZWebFileManager_ShowElement('" + submenuClientId + "')");
                    writer.AddAttribute("onmouseout", "IZWebFileManager_HideElement('" + submenuClientId + "')");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Td);


                itemControl.RenderControl(writer);

                if(hasChildren)
                {
                    RenderDropDownMenu(writer, item.ChildItems, submenuClientId);
                }
                
                writer.RenderEndTag();
            }

        }
    }
}
