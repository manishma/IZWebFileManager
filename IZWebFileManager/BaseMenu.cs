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
    class BaseMenu : Menu
    {
        public new readonly Style DynamicMenuItemStyle;
        public new readonly Style DynamicHoverStyle;
        public new readonly SubMenuStyle DynamicMenuStyle;

        private readonly Action<HtmlTextWriter, MenuItem> renderDynamicItem;

        public readonly bool IsRightToLeft;

        public BaseMenu(bool isRightToLeft, Action<HtmlTextWriter, MenuItem> renderDynamicItem, SubMenuStyle dynamicMenuStyle, Style dynamicMenuItemStyle, Style dynamicHoverStyle)
        {
            IsRightToLeft = isRightToLeft;
            this.renderDynamicItem = renderDynamicItem;
            DynamicMenuItemStyle = dynamicMenuItemStyle;
            DynamicHoverStyle = dynamicHoverStyle;
            DynamicMenuStyle = dynamicMenuStyle;
        }

        protected override ControlAdapter ResolveAdapter()
        {
            return null;
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

            renderDynamicItem(writer, item);

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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var script =
@"

(function() {

var isAChildOf = function (_parent, _child)
{
   if (_parent === _child) { return false; }
      while (_child && _child !== _parent)
   { _child = _child.parentNode; }

   return _child === _parent;
};

var mouseHandler = function (el, event, fn) {
    var relTarget = event.relatedTarget;
    if (el === relTarget || isAChildOf(el, relTarget))
        { return; }
    fn();
};

IZWebFileManager_MouseHover = function(el, event, id) {
    event = event || wondow.event;
    var fn = function() {
        IZWebFileManager_ShowElement (id);
    }
    mouseHandler(el, event, fn);
};

IZWebFileManager_MouseOut = function(el, event, id) {
    event = event || wondow.event;
    var fn = function() {
        IZWebFileManager_HideElement (id);
    }
    mouseHandler(el, event, fn);
};

IZWebFileManager_ShowElement = function(id) {
    var el = WebForm_GetElementById(id);
    el.style.display = 'block';
};

IZWebFileManager_HideElement = function(id) {
    var el = WebForm_GetElementById(id);
    el.style.display = 'none';
};

})();

";
            Page.ClientScript.RegisterClientScriptBlock(typeof(ToolbarMenu), "show_hide_submenu", script, true);
        }
    }
}
