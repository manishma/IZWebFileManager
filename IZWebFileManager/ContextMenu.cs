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
        public ContextMenu(string clientId, bool isRightToLeft, Action<HtmlTextWriter, MenuItem, int> renderDynamicItem, SubMenuStyle dynamicMenuStyle, Style dynamicMenuItemStyle, Style dynamicHoverStyle)
            : base(clientId, isRightToLeft, renderDynamicItem, dynamicMenuStyle, dynamicMenuItemStyle, dynamicHoverStyle)
        {
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "fixed");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "100");
            writer.AddAttribute (HtmlTextWriterAttribute.Id, ClientID);

            var submenuClientId = ClientID + "_0";

            writer.AddAttribute("onmouseover", "IZWebFileManager_MouseHover(this, event, '" + submenuClientId + "')");
            writer.AddAttribute("onmouseout", "IZWebFileManager_MouseOut(this, event, '" + submenuClientId + "')");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            RenderDropDownMenu(writer, Items[0].ChildItems, submenuClientId);

            writer.RenderEndTag ();
        }

    }
}
