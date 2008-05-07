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
using System.ComponentModel;

namespace IZ.WebFileManager
{
	[TypeConverter (typeof (ExpandableObjectConverter))]
	public sealed class ToolbarOptions
	{
		[Flags]
		enum OptionsEnum
		{
			ShowAll = 0xFF,
			ShowCopyButton = 0x01,
			ShowMoveButton = 0x02,
			ShowDeleteButton = 0x04,
			ShowRenameButton = 0x08,
			ShowFolderUpButton = 0x10,
			ShowViewButton = 0x20,
			ShowNewFolderButton = 0x40,
			ShowRefreshButton = 0x80,
		}

		internal ToolbarOptions (StateBag viewState) {
			_viewState = viewState;
		}

		StateBag _viewState;

		OptionsEnum Options {
			get { return _viewState ["ToolbarOptions"] == null ? OptionsEnum.ShowAll : (OptionsEnum) _viewState ["ToolbarOptions"]; }
			set { _viewState ["ToolbarOptions"] = value; }
		}

		[DefaultValue (true)]
		public bool ShowCopyButton {
			get { return (Options & OptionsEnum.ShowCopyButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowCopyButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowCopyButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowMoveButton {
			get { return (Options & OptionsEnum.ShowMoveButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowMoveButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowMoveButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowDeleteButton {
			get { return (Options & OptionsEnum.ShowDeleteButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowDeleteButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowDeleteButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowFolderUpButton {
			get { return (Options & OptionsEnum.ShowFolderUpButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowFolderUpButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowFolderUpButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowRenameButton {
			get { return (Options & OptionsEnum.ShowRenameButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowRenameButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowRenameButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowViewButton {
			get { return (Options & OptionsEnum.ShowViewButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowViewButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowViewButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowNewFolderButton {
			get { return (Options & OptionsEnum.ShowNewFolderButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowNewFolderButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowNewFolderButton;
			}
		}

		[DefaultValue (true)]
		public bool ShowRefreshButton {
			get { return (Options & OptionsEnum.ShowRefreshButton) > 0; }
			set {
				if (value)
					Options |= OptionsEnum.ShowRefreshButton;
				else
					Options &= OptionsEnum.ShowAll ^ OptionsEnum.ShowRefreshButton;
			}
		}
	}
}
