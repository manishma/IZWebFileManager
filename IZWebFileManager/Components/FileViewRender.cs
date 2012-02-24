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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;

namespace IZ.WebFileManager.Components
{
	abstract class FileViewRender
	{
		//protected SortMode sort;
		//protected SortDirection sortDirection;
		protected FileManagerController controller;
		protected FileView fileView;

		protected FileViewRender (FileView fileView) {
			this.fileView = fileView;
			this.controller = fileView.Controller;
			//this.sort = fileView.Sort;
			//this.sortDirection = fileView.SortDirection;
		}


		internal virtual void RenderBeginList (HtmlTextWriter output) {
		}
		internal virtual void RenderEndList (HtmlTextWriter output) {
		}

		internal virtual void RenderBeginGroup (HtmlTextWriter output, GroupInfo group) {
			output.AddStyleAttribute ("clear", "both");
			output.AddStyleAttribute (HtmlTextWriterStyle.Width, "100%");
			output.RenderBeginTag (HtmlTextWriterTag.Div);
			output.Write (HttpUtility.HtmlEncode (group.Name));
			output.RenderEndTag ();
		}
		internal virtual void RenderEndGroup (HtmlTextWriter output, GroupInfo group) {
		}

		//internal virtual void RenderUpDirectory(HtmlTextWriter output, System.IO.DirectoryInfo dir)
		//{
		//}
		internal virtual void RenderItem (HtmlTextWriter output, FileViewItem item) {
		}
		internal static FileViewRender GetRender (FileView fileView) {
			switch (fileView.View) {
			case FileViewRenderMode.Details:
				return new FileViewDetailsRender (fileView);
			case FileViewRenderMode.Icons:
				return new FileViewIconsRender (fileView);
			case FileViewRenderMode.Thumbnails:
    			return new FileViewThumbnailsRender (fileView);
			default:
				return new FileViewDetailsRender (fileView);
			}
		}

		protected void RenderItemName (HtmlTextWriter output, FileViewItem item) {
			if (fileView.UseLinkToOpenItem) {
				string href = item.IsDirectory ?
					"javascript:WFM_" + fileView.Controller.ClientID + ".OnExecuteCommand(WFM_" + fileView.ClientID + ",\'0:0\')" :
                    UrlPathEncode(VirtualPathUtility.AppendTrailingSlash(fileView.CurrentDirectory.VirtualPath) + item.FileSystemInfo.Name);
				if (!item.IsDirectory && !String.IsNullOrEmpty (fileView.LinkToOpenItemTarget))
					output.AddAttribute (HtmlTextWriterAttribute.Target, fileView.LinkToOpenItemTarget);
				output.AddAttribute (HtmlTextWriterAttribute.Href, href, true);
				output.AddAttribute (HtmlTextWriterAttribute.Class, fileView.LinkToOpenItemClass);
				output.RenderBeginTag (HtmlTextWriterTag.A);
				output.Write (HttpUtility.HtmlEncode (item.Name));
				output.RenderEndTag ();
			}
			else {
				output.Write (HttpUtility.HtmlEncode (item.Name));
			}
		}

        static string  UrlPathEncode(string path)
        {
            return HttpUtility.UrlPathEncode(path)
                .Replace("+", "%2b")
                .Replace("#", "%23");
        }
	}

	public enum FileViewRenderMode
	{
		Icons,
		Details,
		Thumbnails
	}

}
