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
using Legend.Web;

namespace IZ.WebFileManager
{
    internal class ToolbarMenu : BaseMenu
    {
        private readonly Action<HtmlTextWriter, MenuItem, int> renderToolbarItem;

        public ToolbarMenu(bool isRightToLeft, Action<HtmlTextWriter, MenuItem, int> renderToolbarItem, Action<HtmlTextWriter, MenuItem, int> renderDynamicItem, SubMenuStyle dynamicMenuStyle, Style dynamicMenuItemStyle, Style dynamicHoverStyle)
            : base(isRightToLeft, renderDynamicItem, dynamicMenuStyle, dynamicMenuItemStyle, dynamicHoverStyle)
        {
            this.renderToolbarItem = renderToolbarItem;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer
                .Tabel(e => e.Cellpadding(0).Cellspacing(0).Border(0))
                    .Tr();

            for (int i = 0; i < Items.Count; i++)
            {
                RenderItem(writer, Items[i], i);
            }

            writer
                    .EndTag()
                .EndTag();

        }
        
        private void RenderItem(HtmlTextWriter writer, MenuItem item, int position)
        {
            var hasChildren = item.ChildItems.Count > 0;
            var submenuClientId = ClientID + "_" + position;

            if (hasChildren)
            {
                writer.AddAttribute("onmouseover", "IZWebFileManager_MouseHover(this, event, '" + submenuClientId + "')");
                writer.AddAttribute("onmouseout", "IZWebFileManager_MouseOut(this, event, '" + submenuClientId + "')");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            renderToolbarItem(writer, item, position);

            if (hasChildren)
            {
                RenderDropDownMenu(writer, item.ChildItems, submenuClientId);
            }

            writer.RenderEndTag();
        }
    }
}
