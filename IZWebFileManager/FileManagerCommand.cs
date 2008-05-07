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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using System.Diagnostics.CodeAnalysis;

namespace IZ.WebFileManager
{
	public sealed class FileManagerCommand : IStateManager
	{
		readonly StateBag bag = new StateBag ();

		//private FileManagerCommandTargets usage = FileManagerCommandTargets.All;
		//[DefaultValue(FileManagerCommandTargets.All)]
		//public FileManagerCommandTargets Usage
		//{
		//    get { return usage; }
		//    set { usage = value; }
		//}

		[DefaultValue ("")]
		public string Name {
			get { return bag ["Name"] == null ? String.Empty : (string) bag ["Name"]; }
			set { bag ["Name"] = value; }
		}

		[DefaultValue ("")]
		public string CommandName {
			get { return bag ["CommandName"] == null ? String.Empty : (string) bag ["CommandName"]; }
			set { bag ["CommandName"] = value; }
		}

		[DefaultValue ("")]
		public string CommandArgument {
			get { return bag ["CommandArgument"] == null ? String.Empty : (string) bag ["CommandArgument"]; }
			set { bag ["CommandArgument"] = value; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string SmallImageUrl {
			get { return bag ["SmallImageUrl"] == null ? String.Empty : (string) bag ["SmallImageUrl"]; }
			set { bag ["SmallImageUrl"] = value; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string LargeImageUrl {
			get { return bag ["LargeImageUrl"] == null ? String.Empty : (string) bag ["LargeImageUrl"]; }
			set { bag ["LargeImageUrl"] = value; }
		}

		internal void SetDirty () {
			bag.SetDirty (true);
		}

		#region IStateManager Members

		bool IStateManager.IsTrackingViewState {
			get { return ((IStateManager) bag).IsTrackingViewState; }
		}

		void IStateManager.LoadViewState (object state) {
			((IStateManager) bag).LoadViewState (state);
		}

		object IStateManager.SaveViewState () {
			return ((IStateManager) bag).SaveViewState ();
		}

		void IStateManager.TrackViewState () {
			((IStateManager) bag).TrackViewState ();
		}

		#endregion
	}
}
