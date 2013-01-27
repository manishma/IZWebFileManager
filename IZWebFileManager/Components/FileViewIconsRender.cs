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
using System.Web.UI;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Globalization;

namespace IZ.WebFileManager.Components
{
	class FileViewIconsRender : FileViewRender
	{
		internal FileViewIconsRender (FileView fileView) : base (fileView) { }

		internal override void RenderItem (System.Web.UI.HtmlTextWriter output, FileViewItem item) {
			output.AddStyleAttribute (HtmlTextWriterStyle.Margin, "1px");
			output.AddStyleAttribute (HtmlTextWriterStyle.Width, "70px");
            output.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
            output.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "top");
			output.RenderBeginTag (HtmlTextWriterTag.Div);

			fileView.RenderItemBeginTag (output, item);

			output.AddStyleAttribute (HtmlTextWriterStyle.Width, "70px");
			output.AddAttribute (HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute (HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute (HtmlTextWriterAttribute.Border, "0");
			output.RenderBeginTag (HtmlTextWriterTag.Table);

			output.RenderBeginTag (HtmlTextWriterTag.Tr);
			output.AddStyleAttribute (HtmlTextWriterStyle.TextAlign, "center");
			output.AddStyleAttribute (HtmlTextWriterStyle.VerticalAlign, "middle");
			output.AddStyleAttribute (HtmlTextWriterStyle.Height, "41px");
			output.AddStyleAttribute (HtmlTextWriterStyle.BackgroundImage, item.LargeImage);
			output.AddStyleAttribute ("background-position", "center center");
			output.AddStyleAttribute ("background-repeat", "no-repeat");
			if (item.Hidden)
				fileView.Controller.HiddenItemStyle.AddAttributesToRender (output);
			output.RenderBeginTag (HtmlTextWriterTag.Td);
			output.RenderEndTag ();
			output.RenderEndTag ();

			output.RenderBeginTag (HtmlTextWriterTag.Tr);
			output.RenderBeginTag (HtmlTextWriterTag.Td);
			output.AddStyleAttribute (HtmlTextWriterStyle.Cursor, "default");
			output.AddStyleAttribute (HtmlTextWriterStyle.Width, "70px");
            output.AddStyleAttribute(HtmlTextWriterStyle.PaddingBottom, "2px");
			output.AddStyleAttribute (HtmlTextWriterStyle.Overflow, "hidden");
			output.AddStyleAttribute (HtmlTextWriterStyle.TextAlign, "center");
			output.AddAttribute (HtmlTextWriterAttribute.Id, item.ClientID + "_Name");
			output.RenderBeginTag (HtmlTextWriterTag.Div);
			RenderItemName (output, item);
			output.RenderEndTag ();
			output.RenderEndTag ();
			output.RenderEndTag ();

			output.RenderEndTag ();

			fileView.RenderItemEndTag (output);

			output.RenderEndTag ();
		}
	}
}
