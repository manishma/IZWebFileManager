// Copyright (C) 2010 Igor Zelmanovich <izwebfilemanager@gmail.com>
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
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;
using System.Reflection;
using System.Web.UI;

namespace IZ.WebFileManager
{
    class BaseMenu
    {
        protected readonly Style DynamicMenuItemStyle;
        protected readonly Style DynamicHoverStyle;
        protected readonly SubMenuStyle DynamicMenuStyle;
        public readonly string ClientID;

        public readonly List<MenuItem> Items = new List<MenuItem>();

        private readonly Action<HtmlTextWriter, MenuItem, int> renderDynamicItem;

        public readonly bool IsRightToLeft;

        public BaseMenu(string clientId, bool isRightToLeft, Action<HtmlTextWriter, MenuItem, int> renderDynamicItem, SubMenuStyle dynamicMenuStyle, Style dynamicMenuItemStyle, Style dynamicHoverStyle)
        {
            IsRightToLeft = isRightToLeft;
            this.renderDynamicItem = renderDynamicItem;
            DynamicMenuItemStyle = dynamicMenuItemStyle;
            DynamicHoverStyle = dynamicHoverStyle;
            DynamicMenuStyle = dynamicMenuStyle;
            ClientID = clientId;
        }

        protected void RenderDropDownMenu(HtmlTextWriter writer, MenuItemCollection items, string submenuClientId)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            DynamicMenuStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, submenuClientId);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            for (int i = 0; i < items.Count; i++)
            {
                var childItem = items[i];
                RenderDropDownMenuItem(writer, childItem, submenuClientId, i);
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected void RenderDropDownMenuItem(HtmlTextWriter writer, MenuItem item, string parentSubmenuClientId, int index)
        {
            var hasChildren = item.ChildItems.Count > 0;
            var submenuClientId = parentSubmenuClientId + "_" + index;

            DynamicMenuItemStyle.AddAttributesToRender(writer);
            if (item.Text != "__separator__")
            {
                var onmouseover = "WebForm_AppendToClassName(this, '" + DynamicHoverStyle.RegisteredCssClass + "');";
                if (hasChildren)
                    onmouseover += "IZWebFileManager_MouseHover(this, event, '" + submenuClientId + "');";
                writer.AddAttribute("onmouseover", onmouseover);

                var onmouseout = "WebForm_RemoveClassName(this, '" + DynamicHoverStyle.RegisteredCssClass + "');";
                if (hasChildren)
                    onmouseout += "IZWebFileManager_MouseOut(this, event, '" + submenuClientId + "');";
                writer.AddAttribute("onmouseout", onmouseout);
                
                writer.AddAttribute("onclick", "IZWebFileManager_HideElement('" + parentSubmenuClientId + "')");
            }
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "1px 0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            renderDynamicItem(writer, item, index);

            if (hasChildren)
            {
                writer.AddStyleAttribute(IsRightToLeft ? "left" : "right", "0");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Top, "0");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderDropDownMenu(writer, item.ChildItems, submenuClientId);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
    }
}
