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
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;

namespace IZ.WebFileManager
{
	public sealed class SpecialFolder : IStateManager
	{
		readonly StateBag bag = new StateBag ();

		[DefaultValue ("")]
		public string DirectoryPath {
			get { return bag ["DirectoryPath"] == null ? String.Empty : (string) bag ["DirectoryPath"]; }
			set { bag ["DirectoryPath"] = value; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string SmallImageUrl {
			get { return bag ["SmallIconUrl"] == null ? String.Empty : (string) bag ["SmallIconUrl"]; }
			set { bag ["SmallIconUrl"] = value; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string LargeImageUrl {
			get { return bag ["LargeIconUrl"] == null ? String.Empty : (string) bag ["LargeIconUrl"]; }
			set { bag ["LargeIconUrl"] = value; }
		}

		[DefaultValue ("Root Folder")]
		public string Text {
			get { return bag ["Text"] == null ? "Root Folder" : (string) bag ["Text"]; }
			set { bag ["Text"] = value; }
		}

		public override string ToString () {
			return DirectoryPath;
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

		internal void SetDirty () {
			bag.SetDirty (true);
		}
	}
}
