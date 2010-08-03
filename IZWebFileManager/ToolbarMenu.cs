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

        public ToolbarMenu(bool isRightToLeft, Action<HtmlTextWriter, MenuItem> renderToolbarItem, Action<HtmlTextWriter, MenuItem> renderDynamicItem)
            : base (isRightToLeft)
        {
            toolbarMenuAdapter = new ToolbarMenuAdapter(this, renderToolbarItem, renderDynamicItem);
        }

        protected override ControlAdapter ResolveAdapter()
        {
            return toolbarMenuAdapter;
        }

        internal class ToolbarMenuAdapter : BaseMenuAdapter
        {
            private readonly Action<HtmlTextWriter, MenuItem> renderToolbarItem;
            
            public ToolbarMenuAdapter(ToolbarMenu toolbarMenu, Action<HtmlTextWriter, MenuItem> renderToolbarItem, Action<HtmlTextWriter, MenuItem> renderDynamicItem)
                : base(toolbarMenu, renderDynamicItem)
            {
                this.renderToolbarItem = renderToolbarItem;
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
                var submenuClientId = Control.ClientID + "_" + position;

                if(hasChildren)
                {
                    writer.AddAttribute("onmouseover", "IZWebFileManager_MouseHover(this, event, '" + submenuClientId + "')");
                    writer.AddAttribute("onmouseout", "IZWebFileManager_MouseOut(this, event, '" + submenuClientId + "')");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                renderToolbarItem(writer, item);

                if(hasChildren)
                {
                    RenderDropDownMenu(writer, item.ChildItems, submenuClientId);
                }
                
                writer.RenderEndTag();
            }
        }
    }
}
